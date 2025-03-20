using System.ComponentModel.DataAnnotations;
using STEPIFY.DTOs;
using STEPIFY.Models.Product_Model;

namespace STEPIFY.Models
{
    public class ProductCategories
    {
        [Key]
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public virtual List<Products>? Products { get; set; }
    }
}
