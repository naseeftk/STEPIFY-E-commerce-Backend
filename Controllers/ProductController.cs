using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEPIFY.Interfaces.IProduct;
using STEPIFY.Models;
using STEPIFY.Models.Product_Model.DTOs;
using STEPIFY.Service;

namespace STEPIFY.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _product;
        public ProductController(IProductService productService)
        {
            _product = productService;
        }



        [HttpGet("AllProducts")]
        public async Task<ActionResult> GetAllProducts()
        {
            var AllProducts = await _product.GetAllProducts();
            return StatusCode(AllProducts.StatusCode, AllProducts);
        }


        [HttpGet("SpecificProduct")]
        public async Task<ActionResult> GetProductById(int productId)
        {
            var OneProduct = await _product.GetProductById(productId);
            return StatusCode(OneProduct.StatusCode, OneProduct);
        }
        [HttpGet("SearchProduct")]

        public async Task<ActionResult> GetSearched(string name)
        {
            var SearchedProduct = await _product.GetSearched(name);
            return StatusCode(SearchedProduct.StatusCode, SearchedProduct);

        }

        [HttpGet("CategorisedProduct")]
        public async Task<ActionResult> GetCategorisedProducts(int idCategory)
        {
            var CategorisedProduct = await _product.GetCategorisedProducts(idCategory);
            return StatusCode(CategorisedProduct.StatusCode, CategorisedProduct);
        }

        [HttpGet("PaginatedProducts")]
        public async Task<ActionResult> GetPaginated(int PageNum, int PageSize)
        {
            var PaginatedProduct = await _product.GetPaginated(PageNum, PageSize);
            return StatusCode(PaginatedProduct.StatusCode, PaginatedProduct);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddProduct")]
        public async Task<ActionResult> AddProduct([FromForm]ProductAddDTO productAddData)
        {
            int adminId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var ProductAdded = await _product.AddProduct(productAddData, adminId);
            return StatusCode(ProductAdded.StatusCode, ProductAdded);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct(int id,[FromForm] ProductAddDTO productData)
        {
            int adminId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var ProductUpdated = await _product.UpdateProduct(id, productData, adminId);
            return StatusCode(ProductUpdated.StatusCode, ProductUpdated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            int adminId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var ProductDeleted = await _product.DeleteProduct(productId, adminId);
            return StatusCode(ProductDeleted.StatusCode, ProductDeleted);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("ProductViewAdmin")]
        public async Task<ActionResult> AdminViewProducts()
        {
            var AllProducts = await _product.AdminViewProducts();
            return StatusCode(AllProducts.StatusCode, AllProducts);
        }
    }
}
