namespace STEPIFY.Models.Product_Model.DTOs
{
    public class AdminProductViewDTO
    {

        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }

        public string Color { get; set; }
        public string Seller { get; set; }
        public string Brand { get; set; }
        public string Details { get; set; }
        public bool IsDeleted { get; set; }

        public string? CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? ModifiedByName { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}

