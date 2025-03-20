using STEPIFY.Models.User_Model;

namespace STEPIFY.Interfaces.Authentication
{
    public interface IAuthenticationRepo
    {

        Task<Users?> GetUserByUsernameOrEmail(string username, string email);
        Task<Users?> GetUserByUserEmail(string Email);
        Task<bool> IsUserBlocked(string Email);
        Task<bool> VerifyPassword(string inputPassword, string storedHashedPassword);
        Task AddUser(Users user);
        Task SaveChangesAsync();
        Task<Users?> GetUserByRefreshToken(string refreshToken);
    }
}



