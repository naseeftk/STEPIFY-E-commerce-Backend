using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Models.Cart_Model.DTOs;

public interface ICartService
{
    Task<ResultDTO<TotalCartDTO>> GetCartItems(int userId);
    Task<ResultDTO<object>> AddToCart(int userId, int productId);
    Task<ResultDTO<object>> RemoveFromCart(int userId, int productId);
    Task<ResultDTO<object>> IncreaseQty(int userId, int productId);
    Task<ResultDTO<object>> DecreaseQty(int userId, int productId);
    Task<ResultDTO<object>> RemoveAllItems(int userId);
}
