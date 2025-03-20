using AutoMapper;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Interfaces.IUser;
using STEPIFY.Models;
using STEPIFY.Models.User_Model.DTOs;
using STEPIFY.Repositories;

namespace STEPIFY.Service
{


    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<ResultDTO<List<UserViewDTO>>> GetAll()
        {
            
            var users = await _userRepo.GetAllUsers();
            var mappedUsers = _mapper.Map<List<UserViewDTO>>(users);
            return new ResultDTO<List<UserViewDTO>>
            {
                Data = mappedUsers,
                StatusCode = 200,
                Message = "Users retrieved successfully"
            };
        }

        public async Task<ResultDTO<UserViewDTO>> GetSpecific(int id)
        {
            var user = await _userRepo.GetUserById(id);
            if (user == null)
            {
                return new ResultDTO<UserViewDTO>
                {
                    Data = null,
                    StatusCode = 404,
                    Message = "User not found"
                };
            }

            var mappedUser = _mapper.Map<UserViewDTO>(user);
            return new ResultDTO<UserViewDTO>
            {
                Data = mappedUser,
                StatusCode = 200,
                Message = $"User with ID {id} retrieved successfully"
            };
        }

        public async Task<ResultDTO<bool>> ToBlockUser(int userId)
        {
            var user = await _userRepo.GetUserById(userId);
            if (user == null)
            {
                return new ResultDTO<bool> { Data = false, Message = "User not found", StatusCode = 404 };
            }

            user.IsBlocked = !user.IsBlocked; // Toggle block status
            var isUpdated = await _userRepo.UpdateUser(user);

            if (!isUpdated)
            {
                return new ResultDTO<bool> { Data = false, Message = "Failed to update user status", StatusCode = 500 };
            }

            return new ResultDTO<bool>
            {
                Data = true,
                Message = user.IsBlocked ? "User Blocked Successfully" : "User Unblocked Successfully",
                StatusCode = 200
            };
        }
    }
}
