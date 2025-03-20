using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using STEPIFY.Models.WishList_Model;
using STEPIFY.Models.CartItems_Model;
using STEPIFY.Models.User_Model;

namespace STEPIFY.Models.Product_Model
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]  // Temporary field
        public int TempField { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = " Price must be greater than or equal to 0")]
        public decimal Price { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Old Price must be greater than or equal to 0")]
        public decimal? OldPrice { get; set; }

        public string Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string Seller { get; set; }

        [Required]
        public string Brand { get; set; }
        public string Color { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual Users? CreatedByAdmin { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual Users? ModifiedByAdmin { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal 0")]
        public int Stock { get; set; }

        public string Details { get; set; }
        public ProductCategories ProductCategory { get; set; }
        public WishList? WishList { get; set; }
        public virtual List<CartItems>? CartItems { get; set; }
        public virtual List<OrderItems>? OrderItems { get; set; } = new List<OrderItems>();

    }
}