using STEPIFY.Models.Order_Model;
using STEPIFY.Models.Product_Model;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderItems
{
    public int Id { get; set; }

    [ForeignKey("Product")]
    public int? ProductId { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }

    public virtual Products? Product { get; set; }
    public virtual Order? Order { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
