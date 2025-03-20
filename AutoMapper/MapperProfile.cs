using AutoMapper;
using STEPIFY.DTOs;
using STEPIFY.DTOs.LoginDTOs;
using STEPIFY.DTOs.RequestDTOs;
using STEPIFY.Models;
using STEPIFY.Models.Address_Model;
using STEPIFY.Models.Address_Model.DTOs;
using STEPIFY.Models.Cart_Model.DTOs;
using STEPIFY.Models.CartItems_Model;
using STEPIFY.Models.CartItems_Model.DTOs;
using STEPIFY.Models.Order_Model;
using STEPIFY.Models.Order_Model.DTOs;
using STEPIFY.Models.OrderItem_Model.DTOs;
using STEPIFY.Models.Product_Model;
using STEPIFY.Models.Product_Model.DTOs;
using STEPIFY.Models.ProductCategory_Model.DTOs;
using STEPIFY.Models.User_Model;
using STEPIFY.Models.User_Model.DTOs;
using STEPIFY.Models.WishList_Model;
using STEPIFY.Models.WishList_Model.DTOs;

namespace STEPIFY.AutoMapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
        CreateMap<Users,UserViewDTO>();
            CreateMap<RegisterDTO,Users>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<Users, RegisterDTO>();    
            CreateMap<Users,LoginDTO>();
            CreateMap<Products, ProductViewDTO>().ForMember(dest=>dest.Category,opt=>  opt.MapFrom(src => src.ProductCategory.CategoryName));
            CreateMap<Products, AdminProductViewDTO>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ProductCategory.CategoryName)).ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByAdmin.UserName)).ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(src => src.ModifiedByAdmin.UserName)).ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate));
            CreateMap<Users, LoginResponseDTO>();

            CreateMap<WishList, WishListDTO>()
                .ForMember(dest => dest.ProductName, opt =>
                    opt.MapFrom(src => src.Product.ProductName)).ForMember(dest => dest.ProductId, opt =>
                    opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.Image, opt =>
                    opt.MapFrom(src => src.Product.Image))
                .ForMember(dest => dest.Price, opt =>
                    opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Brand, opt =>
                    opt.MapFrom(src => src.Product.Brand))
                .ForMember(dest => dest.Category, opt =>
                    opt.MapFrom(src => src.Product.ProductCategory.CategoryName))
                    .ForMember(dest => dest.Details, opt =>
                    opt.MapFrom(src => src.Product.Details))
                .ForMember(dest => dest.Color, opt =>
                    opt.MapFrom(src => src.Product.Color));
              
         



        //CreateMap <Products, ProductAddDTO>().ForMember(dest => dest.Image, opt => opt.Ignore());
        //CreateMap<Products, ProductViewDTO>()  .ForMember(dest => dest.Category,     opt => opt.MapFrom(src => src.ProductCategory.CategoryName));

        CreateMap<ProductAddDTO, ProductViewDTO>().ForMember(dest => dest.Image, opt => opt.Ignore()).ForMember(dest => dest.Category, opt => opt.Ignore());
            CreateMap<Products,WishListDTO>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ProductCategory.CategoryName));
            CreateMap <ProductAddDTO,Products>().ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<Products, WishListDTO>();
            CreateMap<ProductCategories, ProductCategoryDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Products));
            CreateMap<CartItemViewDTO, TotalCartDTO>();
            CreateMap<CartItems, CartItemViewDTO>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Products.ProductName)).ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Products.Price)).ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Products.Image)).ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Quantity * src.Products.Price));
            CreateMap<Add_AddressDTO, Address >();
            CreateMap< Address,Add_AddressDTO > ();
            CreateMap<Address,AddressViewDTO>();
            CreateMap<Order, ViewUserOrderDTO>().ForMember(dest=>dest.UserId,opt=>opt.MapFrom(src=>src.UserId)).ForMember(dest=>dest.TotalAmount,opt=>opt.MapFrom(src=>src.OrderItems.Sum(x=>x.TotalPrice)))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Address.FullName)).ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Address.PhoneNumber))
            .ForMember(dest => dest.AddressDetails, opt => opt.MapFrom(src =>
                src.Address != null ? $"{src.Address.Place}, {src.Address.Pincode}, {src.Address.LandMark}" : "No address details"))
            .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItems, OrderItemViewDTO>().ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName)).ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product.Image));
            CreateMap<OrderItems, OrderItemViewDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : src.ProductName))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : src.ProductImage))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));







        }

    }
}
