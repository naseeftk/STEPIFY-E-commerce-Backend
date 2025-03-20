using STEPIFY.Service;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using STEPIFY.Models.Address_Model;
using STEPIFY.Models.User_Model;
namespace STEPIFY.Models.Order_Model
{
    public class Order
    {

        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
        [Required]
        [ForeignKey("Address")]
        public int AddressId { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]



        public string? TransactionId { get; set; }
        public virtual Users? User { get; set; }
        public virtual Address? Address { get; set; }
        public virtual List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }

}
