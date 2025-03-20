using STEPIFY.Models.OrderItem_Model.DTOs;

namespace STEPIFY.Models.Order_Model.DTOs
{
    public class ViewUserOrderDTO
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        // New fields
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressDetails { get; set; }

        public List<OrderItemViewDTO>? OrderProducts { get; set; }
    }
}
