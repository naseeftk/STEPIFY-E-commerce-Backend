using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEPIFY.DTOs;
using STEPIFY.Service;

namespace STEPIFY.Controllers
{

    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly  ICartService _cart;
        public CartController(ICartService cart)
        {
            _cart = cart;
        }
      
        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _cart.AddToCart(userId, productId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetCartItems")]
        public async Task<IActionResult> GetAllItems()
        {
           
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _cart.GetCartItems(userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _cart.RemoveFromCart(userId, productId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("increase Qty")]
        public async Task<IActionResult> IncreaseQty(int productId)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]); ;
            var result = await _cart.IncreaseQty(userId, productId);
            return StatusCode(result.StatusCode, result);
        } 
        [HttpPut("Decrease Qty")]
        public async Task<IActionResult> DereaseQty(int productId)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _cart.DecreaseQty(userId, productId);
            return StatusCode(result.StatusCode, result);
        } 
        [HttpDelete("Remove All Cart")]
        public async Task<IActionResult> RemoveAll()
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _cart.RemoveAllItems(userId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
