using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Mvc;
using STEPIFY.DTOs;
using STEPIFY.DTOs.RequestDTOs;
using STEPIFY.Interfaces.Authentication;
using STEPIFY.Service;
using System.Threading.Tasks;

namespace STEPIFY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServ _authService;
        public AuthenticationController(IAuthenticationServ authService)
        {
            _authService = authService;
        }


        [HttpPost("SignUp")]
        public async Task<IActionResult> RegisterNewUser(RegisterDTO user)
        {
            var result = await _authService.SignUp(user);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var result = await _authService.Login(loginDto);
            if (result.StatusCode == 200)
            {
                return Ok(new { token = result.Data });
            }
            return StatusCode(result.StatusCode, result);
        }


        // ✅ New endpoint to refresh JWT token
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken( RefreshTokenRequest model)
        {
            var result = await _authService.RefreshToken(model.RefreshToken);
            if (result.StatusCode != 200)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new
            {
                Token = result.Data.Token,
                RefreshToken = result.Data.RefreshToken
            });
        }
    }

}
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }

}
