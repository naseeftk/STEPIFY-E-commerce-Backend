using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Models.Product_Model.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEPIFY.Interfaces.IProduct
{
    public interface IProductService
    {
        Task<ResultDTO<List<AdminProductViewDTO>>> AdminViewProducts();
        Task<ResultDTO<List<ProductViewDTO>>> GetAllProducts();
        Task<ResultDTO<ProductViewDTO>> GetProductById(int productId);
        Task<ResultDTO<List<ProductViewDTO>>> GetPaginated(int PageNum, int PageSize);
        Task<ResultDTO<List<ProductViewDTO>>> GetSearched(string name);
        Task<ResultDTO<object>> AddProduct(ProductAddDTO productAddData, int adminId);
        Task<ResultDTO<object>> UpdateProduct(int id, ProductAddDTO productData, int adminId);
        Task<ResultDTO<object>> DeleteProduct(int productId, int adminId);
        Task<ResultDTO<List<ProductViewDTO>>> GetCategorisedProducts(int idCategory);
    }
}
