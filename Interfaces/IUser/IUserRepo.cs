using STEPIFY.Models.User_Model;

namespace STEPIFY.Interfaces.IUser
{
    public interface IUserRepo
    {
        Task<List<Users>> GetAllUsers();
        Task<Users?> GetUserById(int id);
        Task<bool> UpdateUser(Users user); // Update method to be used for block/unblock
    }
}
