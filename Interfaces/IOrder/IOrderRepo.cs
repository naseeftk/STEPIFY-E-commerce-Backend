
using global::STEPIFY.Models;
using STEPIFY.DTOs;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Models.Cart_Model;
using STEPIFY.Models.Order_Model;
using STEPIFY.Models.Product_Model;

namespace STEPIFY.Interfaces.IOrder
{


    public interface IOrderRepo

        {
  
        Task<List<Order>> GetAllOrders();
            Task<Order?> GetOrderById(int orderId);
            Task<List<Order>> GetOrdersByUserId(int userId);
            Task<bool> AddressExists(int userId, int addressId);
            Task<Cart?> GetUserCart(int userId);
            Task<List<Products>> GetProductsByIds(List<int> productIds);
            Task<bool> SaveChanges();
            Task<decimal> GetTotalRevenue();
            Task AddOrder(Order order);
            Task RemoveCart(Cart cart);
        }
    }


