namespace STEPIFY.Models.OrderItem_Model.DTOs
{
    public class OrderItemViewDTO
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
