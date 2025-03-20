using Microsoft.EntityFrameworkCore;
using STEPIFY.DTOs;
using STEPIFY.Interfaces;
using STEPIFY.Models.Cart_Model;
using STEPIFY.Models.CartItems_Model;
using STEPIFY.Models.Product_Model;
using STEPIFY.Models.User_Model;
using STEPIFY.Service;

namespace STEPIFY.Repositories
{
    public class CartRepo : ICartRepo
    {
        private readonly StepifyDbContext _context;
        private readonly ILogger<CartRepo> _logger;

        public CartRepo(StepifyDbContext context, ILogger<CartRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

 

        public async Task<Cart?> GetCartItems(int userId)
        {
            try
            {
                return await _context.Cart
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Products)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the cart items");
                throw;
            }
        }

        public async Task SaveChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving to the database");
                throw;
            }
        }

        public async Task AddCart(Cart cart)
        {
            try
            {
                _context.Cart.Add(cart);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the cart");
                throw;
            }
        }

        public async Task AddCartItem(CartItems cartItem)
        {
            try
            {
                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the cart item");
                throw;
            }
        }

        public async Task RemoveCartItem(CartItems cartItem)
        {
            try
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the cart item");
                throw;
            }
        }
        public async Task RemoveAllCartItems(List<CartItems> cartItems)
        {
            try
            {
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing all the cart items");
                throw;

            }


        }
        public async Task<Products?> GetProductById(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<Users?> GetUserById(int userId)
        {
            return await _context.Users
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
