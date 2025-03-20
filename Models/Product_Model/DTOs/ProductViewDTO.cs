using System.ComponentModel.DataAnnotations;

namespace STEPIFY.Models.Product_Model.DTOs
{
    public class ProductViewDTO
    {
        public int id { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public string Image { get; set; }


        public string Category { get; set; }

        public string Seller { get; set; }

        public string Brand { get; set; }
        public string Color { get; set; }

        public string Details { get; set; }
    }
}
