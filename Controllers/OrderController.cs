using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEPIFY.DTOs;
using STEPIFY.Interfaces.IOrder;
using STEPIFY.Models;
using STEPIFY.Service;

namespace STEPIFY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService order;
        public OrderController(IOrderService _order)
        {
            order = _order;
        }
        [Authorize]
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(int price)
        {

            var response = await order.RazorOrderCreate(price);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpPost("payment")]
        public async Task<IActionResult> Payment(PaymentDTO razorpay)
        {
            try
            {
                if (razorpay == null)
                {
                    return BadRequest("razorpay details connot be null here");
                }
                var res = await order.RazorPayment(razorpay);
                return StatusCode(res.StatusCode, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {

            var result = await order.GetAllOrders();
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = "User")]
        [HttpGet(" GetSpecificUserOrders")]
        public async Task<IActionResult> GetSpecificUserOrders()
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await order.GetOrdersByUserId(userId);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = "User")]
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder(int addressId,string TransactionId)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await order.PlaceOrder(userId, addressId,TransactionId);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("CalculateRevenue")]
        public async Task<IActionResult> RevenueCalculation()
        {
          
            var result = await order.GetRevenue();
            return StatusCode(result.StatusCode, result);
        }
    }
}
