using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Models.User_Model.DTOs;

namespace STEPIFY.Interfaces.IUser
{
    public interface IUserService
    {
        Task<ResultDTO<List<UserViewDTO>>> GetAll();
        Task<ResultDTO<UserViewDTO>> GetSpecific(int id);
        Task<ResultDTO<bool>> ToBlockUser(int userId);
    }
}
