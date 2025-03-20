using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEPIFY.DTOs;
using STEPIFY.Interfaces.IUser;
using STEPIFY.Models.User_Model;

namespace STEPIFY.Repositories
{


    public class UserRepo : IUserRepo
    {
        private readonly StepifyDbContext _context;

        public UserRepo(StepifyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _context.Users.Where(u => u.Role == "User").ToListAsync();
        }

        public async Task<Users?> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateUser(Users user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
