using STEPIFY.Models.Cart_Model;
using STEPIFY.Models.Product_Model;

namespace STEPIFY.Models.CartItems_Model
{
    public class CartItems
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Products? Products { get; set; }
        public Cart? Cart { get; set; }

    }
}
