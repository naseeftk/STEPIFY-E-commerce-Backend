using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STEPIFY.DTOs;
using STEPIFY.Interfaces.IWishList;
using STEPIFY.Models.WishList_Model;

namespace STEPIFY.Repositories
{
    public class WishListRepo : IWishListRepo
    {
        private readonly StepifyDbContext _context;
        private readonly ILogger<WishListRepo> _logger;
      
        public WishListRepo(StepifyDbContext context, ILogger<WishListRepo> logger)
        {
            _context = context;
            _logger = logger;
   
        }

        public async Task<List<WishList>> GetUserWishList(int userId)
        {
            try
            {
                return await _context.WishList
                    .Include(p => p.Product)
                    .ThenInclude(p => p.ProductCategory)
                    .Where(x => x.UserId == userId)
                      
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching wishlist for user {UserId}", userId);
                throw;
            }
        }

        public async Task<WishList?> GetWishListItem(int userId, int productId)
        {
            try
            {
                return await _context.WishList
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching wishlist item for user {UserId} and product {ProductId}", userId, productId);
                throw;
            }
        }

        public async Task<bool> AddToWishList(WishList wishList)
        {
            try
            {
                await _context.WishList.AddAsync(wishList);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product {ProductId} to wishlist for user {UserId}", wishList.ProductId, wishList.UserId);
                throw;
            }
        }

        public async Task<bool> RemoveFromWishList(WishList wishList)
        {
            try
            {
                _context.WishList.Remove(wishList);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing product {ProductId} from wishlist for user {UserId}", wishList.ProductId, wishList.UserId);
                throw;
            }
        }
    }
}
