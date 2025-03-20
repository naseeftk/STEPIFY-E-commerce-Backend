using Microsoft.EntityFrameworkCore;
using STEPIFY.Interfaces.IProduct;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STEPIFY.Models.Product_Model;

namespace STEPIFY.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly StepifyDbContext _context;
        private readonly ILogger<ProductRepo> _logger;
        public ProductRepo(StepifyDbContext context,ILogger<ProductRepo> logger)
        {
            _context = context;
            _logger= logger;
        }
       public async Task<List<Products>> GetAllProductsToAdmin()
        {
            try
            {
                var allProductsIncludingDeleted = await _context.Products.Include(p => p.ProductCategory).Include(p=>p.CreatedByAdmin).Include(p=>p.ModifiedByAdmin).IgnoreQueryFilters().ToListAsync();

                return allProductsIncludingDeleted;
            }catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while fetching the Admin products");
                throw;
            }
        }
        public async Task<List<Products>> GetAllProducts()
        {
     
            try
            {
                return await _context.Products
                           .Include(p => p.ProductCategory)
                           .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while fetching the products");
                throw;
            }
        }

        public async Task<List<Products>> GetPaginated(int pageNum, int pageSize)
        {
           
            try
            {
                int skip = (pageNum - 1) * pageSize;
                return await _context.Products.Include(p => p.ProductCategory)
                                              .OrderBy(x => x.Id)
                                              .Skip(skip)
                                              .Take(pageSize)
                                              .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while fetching the paginated products");
                throw;
            }
        }

        public async Task<List<Products>> GetSearched(string name)
        {
            
            try
            {
                return await _context.Products.Include(p => p.ProductCategory)
                                           .Where(x => x.ProductName.Contains(name) )
                                           .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while fetching the searched products");
                throw;
            }
        }

        public async Task<bool> AddProduct(Products product)
        {
            try
            {
                var exists = await _context.Products.AnyAsync(x => x.ProductName == product.ProductName);
                if (exists) return false;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while Adding products");
                throw;
            }
           
        }

        public async Task<bool> UpdateProduct(Products product)
        {
            try
            {
             
                _context.Products.Update(product);
                _context.Entry(product).Property(p => p.ModifiedDate).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while updating products");
                throw;
            }

         
        }
        public async Task<bool> DeleteProduct(int productId, int adminId)
        {
            try
            {
               

                var product = await _context.Products.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == productId );
                if (product == null)
                {
                    return false;
                }

                product.IsDeleted = !product.IsDeleted;
                product.ModifiedBy = adminId;
                product.ModifiedDate = DateTime.UtcNow;
                _context.Entry(product).Property(p => p.ModifiedDate).IsModified = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while deleting  product by id");
                throw;
            }

        }

        public async Task<Products?> GetProductById(int id)
        {
            try
            {
                return await _context.Products.Include(p => p.ProductCategory)
                                          .FirstOrDefaultAsync(x => x.Id == id );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while fetching  product by id");
                throw;
            }

           
        }

   

        public async Task<List<Products>> GetCategorisedProducts(int categoryId)
        {
            
            try
            {
                return await _context.Products.Include(p => p.ProductCategory)
                                              .Where(x => x.CategoryId == categoryId)
                                              .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error is occured while fetching categorised product ");
                throw;
            }
        }
    }
}
