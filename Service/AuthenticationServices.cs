using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using AutoMapper;
using STEPIFY.Repositories;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Interfaces.Authentication;
using STEPIFY.DTOs.RequestDTOs;
using STEPIFY.Models.User_Model;
using STEPIFY.DTOs.LoginDTOs;
using STEPIFY.DTOs;

namespace STEPIFY.Services
{
    public class AuthenticationService : IAuthenticationServ
    {
        private readonly IAuthenticationRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationService> _logger;
        public AuthenticationService(IAuthenticationRepo userRepo, IMapper mapper, IConfiguration configuration,ILogger<AuthenticationService> logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _configuration = configuration;
            _logger= logger;
        }

        public async Task<ResultDTO<RegisterDTO>> SignUp(RegisterDTO user)
        {
            try
            {
                var existingUser = await _userRepo.GetUserByUsernameOrEmail(user.UserName, user.Email);
                if (existingUser != null)
                {
                    return new ResultDTO<RegisterDTO> { StatusCode = 409, Message = "User already exists" };
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var newUser = _mapper.Map<Users>(user);
                newUser.Password = hashedPassword;

                await _userRepo.AddUser(newUser);

                return new ResultDTO<RegisterDTO> { StatusCode = 201, Message = "User registered successfully" };
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Error during user registration");
                return new ResultDTO<RegisterDTO> { StatusCode = 500, Message = "An error occurred while registering." };
            }
        }

        public async Task<ResultDTO<LoginResponseDTO>> Login(LoginDTO user)
        {
            try
            {
                var data = await _userRepo.GetUserByUserEmail(user.Email);
                if (data == null)
                {
                    return new ResultDTO<LoginResponseDTO> { StatusCode = 404, Message = "User does not exist" };
                }

                if (await _userRepo.IsUserBlocked(user.Email))
                {
                    return new ResultDTO<LoginResponseDTO> { StatusCode = 403, Message = "User is blocked by admin" };
                }

                bool isPasswordValid = await _userRepo.VerifyPassword(user.Password, data.Password);
                if (!isPasswordValid)
                {
                    return new ResultDTO<LoginResponseDTO> { StatusCode = 401, Message = "Invalid password" };
                }

                var token = GenerateJwtToken(data.Id.ToString(), data.UserName, data.Role);
                var refreshToken = GenerateRefreshToken();

                data.RefreshToken = refreshToken;
                data.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _userRepo.SaveChangesAsync();

                return new ResultDTO<LoginResponseDTO>
                {
                    Data = new LoginResponseDTO
                    {
                        IsBlocked=data.IsBlocked,
                        RefreshTokenExpiry = (DateTime)data.RefreshTokenExpiry,
                        Id =data.Id,
                        Username = data.UserName,
                        Role = data.Role,
                        Token = token,
                        RefreshToken = refreshToken
                    },
                    StatusCode = 200,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return new ResultDTO<LoginResponseDTO> { StatusCode = 500, Message = "An error occurred while logging in" };
            }
        }

        public async Task<ResultDTO<LoginResponseDTO>> RefreshToken(string refreshToken)
        {
            try
            {
                var user = await _userRepo.GetUserByRefreshToken(refreshToken);
                if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    return new ResultDTO<LoginResponseDTO> { StatusCode = 401, Message = "Invalid or expired refresh token" };
                }

                var newAccessToken = GenerateJwtToken(user.Id.ToString(), user.UserName, user.Role);
                var newRefreshToken = GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _userRepo.SaveChangesAsync();

                return new ResultDTO<LoginResponseDTO>
                {
                    Data = new LoginResponseDTO
                    {
                        Username = user.UserName,
                        Role = user.Role,
                        Token = newAccessToken,
                        RefreshToken = newRefreshToken
                    },
                    StatusCode = 200,
                    Message = "Token refreshed successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return new ResultDTO<LoginResponseDTO> { StatusCode = 500, Message = "An error occurred while refreshing token" };
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private string GenerateJwtToken(string userId, string username, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
