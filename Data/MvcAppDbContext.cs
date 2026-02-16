using Mazada.Models;
using Microsoft.EntityFrameworkCore;
namespace Mazada.Data;

public class MvcAppDbContext : DbContext
{
    public MvcAppDbContext(DbContextOptions<MvcAppDbContext> options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Seller> Seller { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<CartItem> CartItem { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<FeaturedProduct> FeaturedProduct { get; set; }
    public DbSet<Section> Section { get; set; }

}