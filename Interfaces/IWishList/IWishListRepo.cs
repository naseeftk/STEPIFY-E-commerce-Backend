using STEPIFY.Models.WishList_Model;

namespace STEPIFY.Interfaces.IWishList
{
    public interface IWishListRepo
    {
        Task<List<WishList>> GetUserWishList(int userId);
        Task<WishList?> GetWishListItem(int userId, int productId);
        Task<bool> AddToWishList(WishList wishList);
        Task<bool> RemoveFromWishList(WishList wishList);
    }
}
