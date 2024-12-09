using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TechAspire.Models
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        #region
        public DbSet<AppUserModel> User {  get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<ProductModel>  Products { get; set; }
        public DbSet<WishListModel> WishLists { get; set; }
        public DbSet<ShippingModel> Shippings { get; set; }

        public DbSet<SliderModel> Sliders { get; set; }
        public DbSet<CouponModel> Coupons { get; set; }
       
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ProductQuantityModel> ProductQuantities { get; set; }

        #endregion
    }
}
