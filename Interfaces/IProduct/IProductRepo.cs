using STEPIFY.Models.Product_Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEPIFY.Interfaces.IProduct
{
    public interface IProductRepo
    {
        Task<List<Products>> GetAllProducts();
        Task<List<Products>> GetAllProductsToAdmin();
        Task<List<Products>> GetPaginated(int PageNum, int PageSize);
        Task<List<Products>> GetSearched(string name);
        Task<bool> AddProduct(Products product);
        Task<bool> UpdateProduct(Products product);
        Task<Products?> GetProductById(int id);
        Task<bool> DeleteProduct(int productId, int adminId);
        Task<List<Products>> GetCategorisedProducts(int idCategory);
    }
}
