using STEPIFY.Repositories;
using Serilog;
using AutoMapper;
using STEPIFY.Models;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Interfaces.IProduct;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Razorpay.Api;
using STEPIFY.Models.Product_Model;
using STEPIFY.Models.Product_Model.DTOs;

namespace STEPIFY.Service
{
    public class ProductService : IProductService   
    {
        private readonly IProductRepo _productRepo;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly StepifyDbContext _context;
        private readonly ICloudinaryService _cloudinary;
        public ProductService(IProductRepo productRepo, ILogger<ProductService> logger, IMapper mapper,StepifyDbContext context, ICloudinaryService cloudinary)
        {
            _productRepo = productRepo;
            _logger = logger;
            _mapper = mapper;
            _context = context;
           _cloudinary=cloudinary;
        }
       public async Task<ResultDTO<List<AdminProductViewDTO>>> AdminViewProducts()
        {
            try
            {
             
              var products= await _productRepo.GetAllProductsToAdmin();
               var mapped= _mapper.Map<List<AdminProductViewDTO>>(products);
                return new ResultDTO<List<AdminProductViewDTO>> { Data = mapped, Message = "products fetched successfully", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Admin products");
                return new ResultDTO<List<AdminProductViewDTO>> { Data = null, Message = "Error fetching products", StatusCode = 500 };
            }
        }
        public async Task<ResultDTO<List<ProductViewDTO>>> GetPaginated(int pageNum, int pageSize)
        {
            try
            {
                var products = await _productRepo.GetPaginated(pageNum, pageSize);
                var mapped = _mapper.Map<List<ProductViewDTO>>(products);

                return new ResultDTO<List<ProductViewDTO>>
                {
                    Data = mapped,
                    StatusCode = 200,
                    Message = "Paginated products retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching paginated products");
                return new ResultDTO<List<ProductViewDTO>>
                {
                    Data = null,
                    StatusCode = 500,
                    Message = "An error occurred"
                };
            }
        }

        public async Task<ResultDTO<List<ProductViewDTO>>> GetCategorisedProducts(int categoryId)
        {
            try
            {
                var products = await _productRepo.GetCategorisedProducts(categoryId);
                var mapped = _mapper.Map<List<ProductViewDTO>>(products);

                return new ResultDTO<List<ProductViewDTO>>
                {
                    Data = mapped,
                    StatusCode = 200,
                    Message = "Categorised products retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categorised products");
                return new ResultDTO<List<ProductViewDTO>>
                {
                    Data = null,
                    StatusCode = 500,
                    Message = "An error occurred"
                };
            }
        }
        public async Task<ResultDTO<List<ProductViewDTO>>> GetSearched(string name)
        {
            try
            {
                var products = await _productRepo.GetSearched(name);
                var mapped = _mapper.Map<List<ProductViewDTO>>(products);

                return new ResultDTO<List<ProductViewDTO>>
                {
                    Data = mapped,
                    StatusCode = 200,
                    Message = "Search results retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching searched products");
                return new ResultDTO<List<ProductViewDTO>>
                {
                    Data = null,
                    StatusCode = 500,
                    Message = "An error occurred"
                };
            }
        }

        public async Task<ResultDTO<ProductViewDTO>> GetProductById(int productId)
        {
            try
            {
                var product = await _productRepo.GetProductById(productId);
                if (product == null)
                    return new ResultDTO<ProductViewDTO> { Data = null, StatusCode = 404, Message = "Product not found" };

                var mapped = _mapper.Map<ProductViewDTO>(product);
                return new ResultDTO<ProductViewDTO> { Data = mapped, StatusCode = 200, Message = "Product retrieved successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID");
                return new ResultDTO<ProductViewDTO> { Data = null, StatusCode = 500, Message = "An error occurred" };
            }
        }

        public async Task<ResultDTO<object>> AddProduct(ProductAddDTO productData, int adminId)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(productData.ProductName))
                {
                    return new ResultDTO<object> { StatusCode = 400, Message = "Product Name is required" };
                }

                // Check if the category exists
                var categoryExists = await _context.ProductCategory.AnyAsync(c => c.Id == productData.CategoryId);
                if (!categoryExists)
                {
                    return new ResultDTO<object> { StatusCode = 400, Message = "Invalid Category ID" };
                }

                // Check for duplicate product
                var exists = await _productRepo.GetSearched(productData.ProductName);
                if (exists.Any())
                    return new ResultDTO<object> { StatusCode = 409, Message = "Product already exists" };

                // Manual Mapping from DTO to Entity
                var productEntity = new Products
                {
                    ProductName = productData.ProductName,
                    Price = productData.Price,
                    OldPrice = productData.OldPrice,
                    Seller = productData.Seller,
                    Brand = productData.Brand,
                    Color = productData.Color,
                    Stock = productData.Stock,
                    Details = productData.Details,
                    CategoryId = productData.CategoryId, // Ensure FK is valid
                    CreatedBy = adminId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate=null,
                    Image = await _cloudinary.UploadImageAsync(productData.Image) // Handle Image Upload
                };

                // Add product and save
                var success = await _productRepo.AddProduct(productEntity);
                if (!success)
                {
                    _logger.LogError("Failed to add product: {@Product}", productEntity);
                    return new ResultDTO<object> { StatusCode = 500, Message = "Failed to add product" };
                }

                _logger.LogInformation("Product added successfully: {@Product}", productEntity);
                return new ResultDTO<object> { StatusCode = 200, Message = "Product added successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product: {@ProductData}", productData);
                return new ResultDTO<object> { StatusCode = 500, Message = "An error occurred while adding" };
            }
        }
    
        public async Task<ResultDTO<List<ProductViewDTO>>> GetAllProducts()
        {
            try
            {
                var products = await _productRepo.GetAllProducts();
                var mapped = _mapper.Map<List<ProductViewDTO>>(products);
                return new ResultDTO<List<ProductViewDTO>> { Data = mapped, StatusCode = 200, Message = "Products retrieved successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                return new ResultDTO<List<ProductViewDTO>> { Data = null, StatusCode = 500, Message = "An error occurred" };
            }
        }

        public async Task<ResultDTO<object>> UpdateProduct(int productId, ProductAddDTO productDto, int adminId)
        {
            try
            {
                var existingProduct = await _productRepo.GetProductById(productId);
                if (existingProduct == null)
                    return new ResultDTO<object> { StatusCode = 404, Message = "Product not found" };

                _mapper.Map(productDto, existingProduct);

                // Upload image to Cloudinary if a new image is provided
                if (productDto.Image != null)
                {
                    var imageUrl = await _cloudinary.UploadImageAsync(productDto.Image);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        existingProduct.Image = imageUrl;
                    }
                }

                existingProduct.ModifiedBy = adminId;
                existingProduct.ModifiedDate = DateTime.UtcNow;
                var success = await _productRepo.UpdateProduct(existingProduct);
                if (!success)
                    return new ResultDTO<object> { StatusCode = 500, Message = "Failed to update product" };

                return new ResultDTO<object> { StatusCode = 200, Message = "Product updated successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                return new ResultDTO<object> { StatusCode = 500, Message = "An error occurred" };
            }
        }

        public async Task<ResultDTO<object>> DeleteProduct(int productId, int adminId)
        {
            try
            {
                var success = await _productRepo.DeleteProduct(productId, adminId);
                if (!success)
                    return new ResultDTO<object> { StatusCode = 404, Message = "Product not found or already deleted" };

                return new ResultDTO<object> { StatusCode = 200, Message = "Product deleted successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return new ResultDTO<object> { StatusCode = 500, Message = "An error occurred" };
            }
        }

     
    }
}
