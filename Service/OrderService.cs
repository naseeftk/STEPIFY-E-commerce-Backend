using STEPIFY.Models;
using STEPIFY.DataAccess;
using AutoMapper;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Interfaces.IOrder;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography;
using System.Text;
using Razorpay.Api;
using STEPIFY.Models.Order_Model.DTOs;
using STEPIFY.DTOs;
namespace STEPIFY.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepo orderRepository, IMapper mapper, ILogger<OrderService> logger,IConfiguration configuration       )
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
//
         
        }
        public async Task<ResultDTO<object>> RazorOrderCreate(int price)
        {
            try
            {
                if (price <= 0)

                {
                    return new ResultDTO<object> { StatusCode = 400, Message = "enter valid price" };
                }
                Dictionary<string, object> input = new Dictionary<string, object>
                {
                    { "amount", price * 100 },
                    { "currency", "INR" },
                    { "receipt", Guid.NewGuid().ToString() }
                };


                string key = "rzp_test_JChdnCdqvPMIoB";
                string secret = "cbz6sk8xnH9H14SpJ4Io8oke";
                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(input);
                string orderId = order["id"].ToString();

                return new ResultDTO<object> { StatusCode = 200, Message = "OrderId Created Successfully", Data = orderId };
            }
            catch (Exception ex)
            {
                return new ResultDTO<object> { StatusCode = 200, Message = ex.Message };
            }
        }


        public async Task<ResultDTO<bool>> RazorPayment(PaymentDTO payment)
        {

            if (payment == null ||
        string.IsNullOrEmpty(payment.razorpay_payment_id) ||
        string.IsNullOrEmpty(payment.razorpay_order_id) ||
        string.IsNullOrEmpty(payment.razorpay_signature))
            {
                return new ResultDTO<bool> { StatusCode = 400, Message = "Credentials not found" };
            }

            try
            {

                string key = "rzp_test_iA2stFg1qD86OQ";
                string secret = "B442j5qkUCP0WrsGGgHBG6F8";

                Dictionary<string, string> attributes = new Dictionary<string, string>
        {
            { "razorpay_payment_id", payment.razorpay_payment_id },
            { "razorpay_order_id", payment.razorpay_order_id },
            { "razorpay_signature", payment.razorpay_signature },
            { "secret", secret }
        };


                Utils.verifyPaymentSignature(attributes);

                return new ResultDTO<bool> { StatusCode = 200, Message = "Payment success", Data = true };
            }
            catch (Razorpay.Api.Errors.SignatureVerificationError ex)
            {
                return new ResultDTO<bool> { StatusCode = 400, Message = "Invalid signature: " + ex.Message };
            }
            catch (Exception ex)
            {
                return new ResultDTO<bool> { StatusCode = 400, Message = "Error while verifying Razorpay payment: " + ex.Message };
            }


        }

        private string GenerateSignature(string paymentId, string orderId, string secret)
        {
            string stringToSign = orderId + "|" + paymentId;
            Console.WriteLine("String to Sign: " + stringToSign);

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
                string generatedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                Console.WriteLine("Generated Signature: " + generatedSignature);
                return generatedSignature;
            }
        }



        public async Task<ResultDTO<List<ViewUserOrderDTO>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrders();
                if (!orders.Any())
                    return new ResultDTO<List<ViewUserOrderDTO>> { StatusCode = 404, Message = "No orders found" };

                return new ResultDTO<List<ViewUserOrderDTO>>
                {
                    StatusCode = 200,
                    Data = _mapper.Map<List<ViewUserOrderDTO>>(orders),
                    Message = "Orders retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all orders");
                return new ResultDTO<List<ViewUserOrderDTO>> { StatusCode = 500, Message = "Server error" };
            }
        }

        public async Task<ResultDTO<object>> PlaceOrder(int userId, int addressId,string TransactionId)
        {
            try
            {
                if (!await _orderRepository.AddressExists(userId, addressId))
                    return new ResultDTO<object> { StatusCode = 400, Message = "Invalid address" };

                var cart = await _orderRepository.GetUserCart(userId);
                if (cart == null || !cart.CartItems.Any())
                    return new ResultDTO<object> { StatusCode = 400, Message = "Cart is empty" };

                var products = await _orderRepository.GetProductsByIds(cart.CartItems.Select(ci => ci.ProductId).ToList());

                foreach (var item in cart.CartItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null || product.Stock < item.Quantity)
                        return new ResultDTO<object> { StatusCode = 400, Message = "Stock issue" };

                    product.Stock -= item.Quantity;
                }

                var order = new STEPIFY.Models.Order
                {
                    UserId = userId,
                    AddressId = addressId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Products.Price),
                    Status = "Delivered",
                    TransactionId = TransactionId,
                    OrderItems = cart.CartItems.Select(ci => new OrderItems
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.Quantity * ci.Products.Price,
                         ProductImage = ci.Products.Image ?? "default-image.png",
                         ProductName= ci.Products?.ProductName ?? "Unknown Product",
                        

                    }).ToList()
                };

                await _orderRepository.AddOrder(order);
                await _orderRepository.RemoveCart(cart);
                await _orderRepository.SaveChanges();

                return new ResultDTO<object> { StatusCode = 200, Message = "Order placed" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error placing order");
                return new ResultDTO<object> { StatusCode = 500, Message = "Server error" };
            }
        }
        public async Task<ResultDTO<List<ViewUserOrderDTO>>> GetOrdersByUserId(int userId)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByUserId(userId);

                if (!orders.Any())
                {
                    return new ResultDTO<List<ViewUserOrderDTO>>
                    {
                        StatusCode = 200,
                        Message = "No orders found for this user"
                    };
                }

                var orderDetails = _mapper.Map<List<ViewUserOrderDTO>>(orders);

                return new ResultDTO<List<ViewUserOrderDTO>>
                {
                    StatusCode = 200,
                    Message = "Orders retrieved successfully",
                    Data = orderDetails
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching orders for user {userId}");
                return new ResultDTO<List<ViewUserOrderDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching orders"
                };
            }
        }

        public async Task<ResultDTO<object>> GetRevenue()
        {
            try
            {
                var revenue = await _orderRepository.GetTotalRevenue();
                return new ResultDTO<object>
                {
                    StatusCode = 200,
                    Message = "Revenue retrieved successfully",
                    Data = revenue
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating revenue");
                return new ResultDTO<object>
                {
                    StatusCode = 500,
                    Message = "An error occurred while calculating revenue"
                };
            }
        }

    }
}
