

using System.ComponentModel.DataAnnotations;
using STEPIFY.Models.Product_Model;
using STEPIFY.Models.User_Model;

namespace STEPIFY.Models.WishList_Model
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public virtual Users? User { get; set; }
        public virtual Products? Product { get; set; }
    }
}
