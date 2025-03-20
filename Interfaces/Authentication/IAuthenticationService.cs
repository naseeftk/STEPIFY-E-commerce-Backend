using STEPIFY.DTOs;
using STEPIFY.DTOs.LoginDTOs;
using STEPIFY.DTOs.RequestDTOs;
using STEPIFY.DTOs.ResultDTO;

namespace STEPIFY.Interfaces.Authentication
{

    public interface IAuthenticationServ
    {
        Task<ResultDTO<RegisterDTO>> SignUp(RegisterDTO user);
        Task<ResultDTO<LoginResponseDTO>> Login(LoginDTO loginDto);
        Task<ResultDTO<LoginResponseDTO>> RefreshToken(string refreshToken);
    }
}

