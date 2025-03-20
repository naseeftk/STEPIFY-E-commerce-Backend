using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Models.WishList_Model.DTOs;

namespace STEPIFY.Interfaces.IWishList
{
    public interface IWishListService
    {
        Task<ResultDTO<List<WishListDTO>>> GetWishList(int userId);
        Task<ResultDTO<string>> AddOrRemoveWishlist(int userId, int productId);
    }
}
