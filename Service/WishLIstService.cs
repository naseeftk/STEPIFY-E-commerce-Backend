using AutoMapper;
using Microsoft.Extensions.Logging;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Interfaces.IProduct;
using STEPIFY.Interfaces.IUser;
using STEPIFY.Interfaces.IWishList;
using STEPIFY.Models.WishList_Model;
using STEPIFY.Models.WishList_Model.DTOs;

namespace STEPIFY.Service
{
    public class WishListService : IWishListService
    {
        private readonly IWishListRepo _wishListRepo;
        private readonly IUserRepo _userRepo;
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<WishListService> _logger;

        public WishListService(IWishListRepo wishListRepo, IUserRepo userRepo, IProductRepo productRepo, IMapper mapper, ILogger<WishListService> logger)
        {
            _wishListRepo = wishListRepo;
            _userRepo = userRepo;
            _productRepo = productRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResultDTO<List<WishListDTO>>> GetWishList(int userId)
        {
            try
            {
                var userExists = await _userRepo.GetUserById(userId);
                if (userExists == null)
                {
                    _logger.LogWarning("User {UserId} not found while fetching wishlist", userId);
                    return new ResultDTO<List<WishListDTO>> { Data = null, StatusCode = 404, Message = "User not found" };
                }

                var wishList = await _wishListRepo.GetUserWishList(userId);
                if (!wishList.Any())
                {
                    _logger.LogInformation("Wishlist is empty for user {UserId}", userId);
                    return new ResultDTO<List<WishListDTO>> { Data = null, StatusCode = 404, Message = "Your WishList is Empty" };
                }

                var mappedWishlist = _mapper.Map<List<WishListDTO>>(wishList);
                return new ResultDTO<List<WishListDTO>> { Data = mappedWishlist, StatusCode = 200, Message = "Wishlist retrieved successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching wishlist for user {UserId}", userId);
                return new ResultDTO<List<WishListDTO>> { Data = null, StatusCode = 500, Message = "An error occurred while fetching wishlist" };
            }
        }

        public async Task<ResultDTO<string>> AddOrRemoveWishlist(int userId, int productId)
        {
            try
            {
                var userExists = await _userRepo.GetUserById(userId);
                if (userExists == null)
                {
                    _logger.LogWarning("User {UserId} not found while modifying wishlist", userId);
                    return new ResultDTO<string> { Data = null, StatusCode = 404, Message = "User not found" };
                }

                var product = await _productRepo.GetProductById(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product {ProductId} not found while modifying wishlist for user {UserId}", productId, userId);
                    return new ResultDTO<string> { Data = null, StatusCode = 404, Message = "Product not found" };
                }

                var existingWishlistItem = await _wishListRepo.GetWishListItem(userId, productId);

                if (existingWishlistItem != null)
                {
                    var removed = await _wishListRepo.RemoveFromWishList(existingWishlistItem);
                    if (removed)
                    {
                        _logger.LogInformation("Product {ProductId} removed from wishlist for user {UserId}", productId, userId);
                        return new ResultDTO<string> { Data = null, StatusCode = 200, Message = "Product removed from wishlist" };
                    }
                    else
                    {
                        _logger.LogError("Failed to remove product {ProductId} from wishlist for user {UserId}", productId, userId);
                        return new ResultDTO<string> { Data = null, StatusCode = 500, Message = "Failed to remove product from wishlist" };
                    }
                }

                var newWishListItem = new WishList { UserId = userId, ProductId = productId };
                var added = await _wishListRepo.AddToWishList(newWishListItem);

                if (added)
                {
                    _logger.LogInformation("Product {ProductId} added to wishlist for user {UserId}", productId, userId);
                    return new ResultDTO<string> { Data = null, StatusCode = 200, Message = "Product added to wishlist" };
                }
                else
                {
                    _logger.LogError("Failed to add product {ProductId} to wishlist for user {UserId}", productId, userId);
                    return new ResultDTO<string> { Data = null, StatusCode = 500, Message = "Failed to add product to wishlist" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while modifying wishlist for user {UserId} and product {ProductId}", userId, productId);
                return new ResultDTO<string> { Data = null, StatusCode = 500, Message = "An error occurred while modifying wishlist" };
            }
        }
    }
}
