using AutoMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Interfaces;
using STEPIFY.Models.Cart_Model.DTOs;
using STEPIFY.Models.CartItems_Model;
using STEPIFY.Models.CartItems_Model.DTOs;
using STEPIFY.Repositories;

namespace STEPIFY.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _cartRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepo cartRepo, IMapper mapper,ILogger<CartService> logger)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResultDTO<TotalCartDTO>> GetCartItems(int userId)
        {
            try
            {
                var userCart = await _cartRepo.GetCartItems(userId);
                if (userCart == null)
                {

                    return new ResultDTO<TotalCartDTO> { Message = "Cart is empty", StatusCode = 404 };
                }

                var cartItems = userCart.CartItems.ToList();
                var mappedData = _mapper.Map<List<CartItemViewDTO>>(cartItems);
                var totalProducts = mappedData.Count();
                var totalAmount = mappedData.Sum(x => x.TotalAmount);

                var finalCart = new TotalCartDTO
                {
                    CartItemsperUser = mappedData,
                    TotalItems = totalProducts,
                    TotalPrice = totalAmount
                };
                return new ResultDTO<TotalCartDTO> { Message = "Cart items retrieved successfully", StatusCode = 200, Data = finalCart };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error ocuured while fetching cart items");
                throw;
            }
           
        }

        public async Task<ResultDTO<object>> AddToCart(int userId, int productId)
        {

            try
            {
                var product = await _cartRepo.GetProductById(productId);
                if (product == null)
                {
                    return new ResultDTO<object> { Message = "Product not found", StatusCode = 404 };
                }

                if (product.Stock == 0)
                {
                    return new ResultDTO<object> { Message = "Out of stock", StatusCode = 400 };
                }

                var user = await _cartRepo.GetUserById(userId);
                if (user == null)
                {
                    return new ResultDTO<object> { Message = "User not found", StatusCode = 404 };
                }

                if (user.Cart == null)
                {
                    user.Cart = new Cart { UserId = userId, CartItems = new List<CartItems>() };
                    await _cartRepo.AddCart(user.Cart);
                }

                var existingItem = user.Cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

                if (existingItem != null)
                {
                    if (existingItem.Quantity >= product.Stock)
                    {
                        return new ResultDTO<object> { Message = "Out of stock", StatusCode = 400 };
                    }

                    existingItem.Quantity++;
                    await _cartRepo.SaveChanges();
                    return new ResultDTO<object> { Message = "Product quantity increased", StatusCode = 200 };
                }

                var newItem = new CartItems
                {
                    ProductId = productId,
                    CartId = user.Cart.Id,
                    Quantity = 1
                };

                await _cartRepo.AddCartItem(newItem);
                return new ResultDTO<object> { StatusCode = 200, Message = "Product added to cart" };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error ocuured while Adding to cart items");
                throw;
            }
        }
          
       

        public async Task<ResultDTO<object>> RemoveFromCart(int userId, int productId)
        {
            try
            {
                var user = await _cartRepo.GetUserById(userId);
                if (user?.Cart == null)
                {
                    return new ResultDTO<object> { Message = "Cart is empty", StatusCode = 404 };
                }

                var cartItem = user.Cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem == null)
                {
                    return new ResultDTO<object> { Message = "Product not found in cart", StatusCode = 404 };
                }

                await _cartRepo.RemoveCartItem(cartItem);
                return new ResultDTO<object> { StatusCode = 200, Message = "Product removed from cart" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error ocuured while removing from cart ");
                throw;
            }
           
        }

        public async Task<ResultDTO<object>> IncreaseQty(int userId, int productId)
        {
            try
            {
                var userCart = await _cartRepo.GetCartItems(userId);
                if (userCart == null)
                {
                    return new ResultDTO<object> { Message = "Cart not found", StatusCode = 404 };
                }

                var cartItem = userCart.CartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem == null)
                {
                    return new ResultDTO<object> { Message = "Product not found in cart", StatusCode = 404 };
                }

                var product = await _cartRepo.GetProductById(productId);
                if (cartItem.Quantity >= product.Stock)
                {
                    return new ResultDTO<object> { Message = "Out of stock", StatusCode = 400 };
                }

                cartItem.Quantity++;
                await _cartRepo.SaveChanges();
                return new ResultDTO<object> { StatusCode = 200, Message = "Product quantity increased" };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "error ocuured while increasing the qty");
                throw;
            }
           
        }
        
        public async Task<ResultDTO<object>> DecreaseQty(int userId, int productId)
        {
            try
            {
                var userCart = await _cartRepo.GetCartItems(userId);
                if (userCart == null)
                {
                    return new ResultDTO<object> { Message = "Cart not found", StatusCode = 404 };
                }

                var cartItem = userCart.CartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem == null)
                {
                    return new ResultDTO<object> { Message = "Product not found in cart", StatusCode = 404 };
                }


                if (cartItem.Quantity == 1)
                {
                    await _cartRepo.RemoveCartItem(cartItem);
                    return new ResultDTO<object> { Message = "Product Removed From Cart", StatusCode = 400 };
                }

                cartItem.Quantity--;
                await _cartRepo.SaveChanges();
                return new ResultDTO<object> { StatusCode = 200, Message = "Product quantity decreased" };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "error ocuured while decreasimg to cart items");
                throw;
            }
         
        }

        public async Task<ResultDTO<object>> RemoveAllItems(int userId)
        {
            try
            {
                var userCart = await _cartRepo.GetCartItems(userId);
                if (userCart?.CartItems == null)
                {
                    return new ResultDTO<object> { Message = "Cart is already empty", StatusCode = 200 };
                }

              
                    await _cartRepo.RemoveAllCartItems(userCart.CartItems);
                

                return new ResultDTO<object> { Message = "All items removed from cart", StatusCode = 200 };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "error ocuured while Removing all cart items");
                throw;
            }
         
        }
    }
}
