using STEPIFY.DTOs;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Models.Order_Model.DTOs;





namespace STEPIFY.Interfaces.IOrder
{
    public interface IOrderService
    {
        Task<ResultDTO<object>> RazorOrderCreate(int price);
        Task<ResultDTO<bool>> RazorPayment(PaymentDTO  payment);
        Task<ResultDTO<List<ViewUserOrderDTO>>> GetAllOrders();
        Task<ResultDTO<object>> PlaceOrder(int userId, int addressId,string TransactionId);
        Task<ResultDTO<List<ViewUserOrderDTO>>> GetOrdersByUserId(int userId);
        Task<ResultDTO<object>> GetRevenue();
    }
}


