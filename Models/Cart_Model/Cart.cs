

using STEPIFY.Models.CartItems_Model;
using STEPIFY.Models.User_Model;

namespace STEPIFY.Models.Cart_Model
{
    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public List<CartItems>? CartItems { get; set; }
        public Users? User { get; set; }
    }
}
