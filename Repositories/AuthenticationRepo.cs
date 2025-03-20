using Microsoft.EntityFrameworkCore;
using Serilog;
using STEPIFY.Interfaces.Authentication;
using STEPIFY.Models.User_Model;  // Import Serilog for logging

namespace STEPIFY.Repositories
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        private readonly StepifyDbContext _context;
        private readonly ILogger<AuthenticationRepo> _logger;
        public AuthenticationRepo(StepifyDbContext context,ILogger<AuthenticationRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Users?> GetUserByUsernameOrEmail(string username, string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username || x.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by username or email");
                throw;
            }
        }

        public async Task<Users?> GetUserByUserEmail(string Email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Email==Email );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by userEmail");
                throw;
            }
        }

        public async Task<bool> IsUserBlocked(string Email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == Email);
                return user != null && user.IsBlocked;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user is blocked");
                throw;
            }
        }

        public async Task<bool> VerifyPassword(string inputPassword, string storedHashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(inputPassword, storedHashedPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password");
                throw;
            }
        }

        public async Task AddUser(Users user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new user");
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to database");
                throw;
            }
        }

        public async Task<Users?> GetUserByRefreshToken(string refreshToken)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by refresh token");
                throw;
            }
        }
    }
}
