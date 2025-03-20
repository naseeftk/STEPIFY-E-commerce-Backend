using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using STEPIFY.Models.Order_Model;
using STEPIFY.Models.WishList_Model;
using STEPIFY.Models.Address_Model;
using STEPIFY.Models.Cart_Model;

namespace STEPIFY.Models.User_Model
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNo { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsBlocked { get; set; } = false;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string Role { get; set; } = "User";

        public List<WishList>? WishLists { get; set; }
        public virtual Cart? Cart { get; set; }
        public List<Order>? Orders { get; set; } = new List<Order>();
        public List<Address>? Address { get; set; }

    }
}
