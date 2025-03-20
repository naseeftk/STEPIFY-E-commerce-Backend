using System.ComponentModel.DataAnnotations;

namespace STEPIFY.Models.Product_Model.DTOs
{
    public class ProductAddDTO


    {

        [Required(ErrorMessage = "Name is Required")]

        public string ProductName { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }
        [Required]
        [Range(0, 100000)]
        public decimal? OldPrice { get; set; }
        [Required]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Category is Required")]
        public int CategoryId { get; set; }
        [Required]
        public string Seller { get; set; }


        [Required(ErrorMessage = "Brand is Required")]
        public string Brand { get; set; }
        [Required]
        public string Color { get; set; }
        [Required(ErrorMessage = "Stock is Required")]
        public int Stock { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Length doesnot exceed 500")]
        public string Details { get; set; }
    }

}

