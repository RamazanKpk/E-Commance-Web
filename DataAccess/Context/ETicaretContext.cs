using Entity;
using System.Data.Entity;

namespace DataAccess.Context
{
    public class ETicaretContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<UserFavoriteProduct> UserFavoriteProducts { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
    }
}
