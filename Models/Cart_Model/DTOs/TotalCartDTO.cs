using STEPIFY.Models.CartItems_Model.DTOs;

namespace STEPIFY.Models.Cart_Model.DTOs
{
    public class TotalCartDTO
    {
        public List<CartItemViewDTO>? CartItemsperUser { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
