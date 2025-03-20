using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEPIFY.Interfaces.IAddress;
using STEPIFY.Service;
using STEPIFY.Models.Address_Model.DTOs;
namespace STEPIFY.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
 
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _address;
        public AddressController(IAddressService address)
        {
            _address = address;
        }
        [HttpGet("GetAllAddress")]
        public async Task<IActionResult> Get()
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _address.GetAllAddress(userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("ToAddnewAddress")]
        public async Task<IActionResult> AddNew(Add_AddressDTO newAddress)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _address.Add_Address(userId, newAddress);
            return StatusCode(result.StatusCode, result);
        }
        
        [HttpDelete("deleteAddress")]
        public async Task<IActionResult> RemoveAddress(int addressId)
        {
            int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var result = await _address.RemoveAddress(userId, addressId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
