using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEPIFY.DTOs;
using STEPIFY.Interfaces.IWishList;
using STEPIFY.Models;
using STEPIFY.Service;

namespace STEPIFY.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishList;
        public WishListController(IWishListService wishList)
        {
            _wishList = wishList;
        }
        [HttpPost("AddOrRemoveFromWishlist")]
        public async Task<IActionResult> AddOrRemove(int productId)
        {
           
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return Unauthorized(new { message = "User ID not found in token" });
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid User ID format" });
            }


            var result = await _wishList.AddOrRemoveWishlist(userId, productId);

            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("getWishlist")]
        public async Task<IActionResult> GetWishList()
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _wishList.GetWishList(userId);

            return StatusCode(result.StatusCode, result);
        }


    }
}
