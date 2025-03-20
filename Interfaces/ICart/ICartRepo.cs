using STEPIFY.DTOs;
using STEPIFY.Models.Cart_Model;
using STEPIFY.Models.CartItems_Model;
using STEPIFY.Models.Product_Model;
using STEPIFY.Models.User_Model;

public interface ICartRepo
{
    Task<Cart?> GetCartItems(int userId);
    Task SaveChanges();
    Task AddCart(Cart cart);
    Task AddCartItem(CartItems cartItem);
    Task RemoveCartItem(CartItems cartItem);
    Task RemoveAllCartItems(List<CartItems> cartItems);

    Task<Users?> GetUserById(int userId);
    Task<Products?> GetProductById(int productId);
}
