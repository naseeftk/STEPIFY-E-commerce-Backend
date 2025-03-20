using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEPIFY.Interfaces.IUser;
using STEPIFY.Service;

namespace STEPIFY.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        public UserController(IUserService user)
        {
            _user = user;
        }
     
        [HttpGet("TogetAllUsers")]
        public async Task<IActionResult> GetFull()
        {
            var FullUsers= await _user.GetAll();
            return StatusCode(FullUsers.StatusCode, FullUsers);
        }
        
        [HttpGet("TogetSpecificUsers")]
        public async Task<IActionResult> GetOne(int id)
        {
            var OneUser= await _user.GetSpecific(id);
            return StatusCode(OneUser.StatusCode, OneUser);
        }
        [HttpPatch("ToBlockUser")]
        public async Task<IActionResult> ToBlock (int id)
        {
            var OneUser = await _user.ToBlockUser(id);
            return StatusCode(OneUser.StatusCode, OneUser);
        }

    }
}
