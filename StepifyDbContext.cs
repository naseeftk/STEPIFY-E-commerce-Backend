using Microsoft.EntityFrameworkCore;
using STEPIFY.Models;
using STEPIFY.Models.Address;
using STEPIFY.Models.Cart;
using STEPIFY.Models.CartItems;
using STEPIFY.Models.Order_Model;
using STEPIFY.Models.Product_Model;
using STEPIFY.Models.User_Model;
using STEPIFY.Models.WishList_Model;

namespace STEPIFY
{
    public class StepifyDbContext : DbContext
    {
        public StepifyDbContext(DbContextOptions<StepifyDbContext> options) : base(options) { }

        public DbSet<Products> Products { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<ProductCategories> ProductCategory { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>().HasQueryFilter(p => !p.IsDeleted);//for removing all deleted from queries

            // Decimal precision for monetary values
            modelBuilder.Entity<Products>().Property(p => p.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Products>().Property(p => p.OldPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(O => O.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<OrderItems>().Property(OI => OI.TotalPrice).HasPrecision(18, 2);

            // Seed Data
            modelBuilder.Entity<ProductCategories>().HasData(
                new ProductCategories { Id = 100, CategoryName = "Sneakers" },
                new ProductCategories { Id = 200, CategoryName = "Sports" },
                new ProductCategories { Id = 300, CategoryName = "Casual" }
            );

            modelBuilder.Entity<Products>()
                .HasOne(P => P.ProductCategory)
                .WithMany(C => C.Products)
                .HasForeignKey(P => P.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Removed Cascade

            modelBuilder.Entity<WishList>()
                .HasOne(W => W.User)
                .WithMany(U => U.WishLists)
                .HasForeignKey(W => W.UserId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Removed Cascade

            modelBuilder.Entity<WishList>()
                .HasOne(W => W.Product)
                .WithOne(P => P.WishList)
                .HasForeignKey<WishList>(W => W.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Removed Cascade

            modelBuilder.Entity<CartItems>()
                .HasOne(CI => CI.Products)
                .WithMany(P => P.CartItems)
                .HasForeignKey(CI => CI.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Removed Cascade

            modelBuilder.Entity<Cart>()
                .HasOne(C => C.User)
                .WithOne(U => U.Cart)
                .HasForeignKey<Cart>(C => C.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItems>()
                .HasOne(CI => CI.Cart)
                .WithMany(C => C.CartItems)
                .HasForeignKey(CI => CI.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Address>()
                .HasOne(A => A.User)
                .WithMany(U => U.Address)
                .HasForeignKey(A => A.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(O => O.User)
                .WithMany(U => U.Orders)
                .HasForeignKey(O => O.UserId)
                .OnDelete(DeleteBehavior.NoAction); // ✅ Prevent cascading delete

            modelBuilder.Entity<Order>()
                .HasOne(O => O.Address)
                .WithMany(A => A.Orders)
                .HasForeignKey(O => O.AddressId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderItems>()
                .HasOne(OI => OI.Order)
                .WithMany(O => O.OrderItems)
                .HasForeignKey(OI => OI.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Removed Cascade

            modelBuilder.Entity<OrderItems>()
                .HasOne(OI => OI.Product)
                .WithMany(P => P.OrderItems)
                .HasForeignKey(OI => OI.ProductId)
                .OnDelete(DeleteBehavior.SetNull); // ✅ Keep order items even if the product is soft deleted
        }
    }
}