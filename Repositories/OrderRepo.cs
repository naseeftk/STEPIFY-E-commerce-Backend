using Microsoft.EntityFrameworkCore;
using STEPIFY.DTOs;
using STEPIFY.Interfaces.IOrder;
using STEPIFY.Interfaces;
using STEPIFY.Models.Cart_Model;
using STEPIFY.Models.Order_Model;
using STEPIFY.Models.Product_Model;

namespace STEPIFY.DataAccess
{
    public class OrderRepository : IOrderRepo
    {
        private readonly StepifyDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(StepifyDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            try
            {
                return await _context.Order
                    .Include(o => o.Address)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all orders");
                return new List<Order>();
            }
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            try
            {
                return await _context.Order.FindAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching order {orderId}");
                return null;
            }
        }

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            try
            {
                return await _context.Order
                    .Include(o => o.Address)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .Where(o => o.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching orders for user {userId}");
                return new List<Order>();
            }
        }

        public async Task<bool> AddressExists(int userId, int addressId)
        {
            return await _context.Address.AnyAsync(a => a.Id == addressId && a.UserId == userId);
        }

        public async Task<Cart?> GetUserCart(int userId)
        {
            return await _context.Cart
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Products)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<Products>> GetProductsByIds(List<int> productIds)
        {
            return await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<decimal> GetTotalRevenue()
        {
            return await _context.OrderItems.SumAsync(oi => oi.TotalPrice);
        }

        public async Task AddOrder(Order order)
        {
            await _context.Order.AddAsync(order);
        }

        public async Task RemoveCart(Cart cart)
        {
            _context.Cart.Remove(cart);
        }
    }
}
