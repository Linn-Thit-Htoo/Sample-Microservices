using Microsoft.EntityFrameworkCore;
using Product.Microservice.Models;

namespace Product.Microservice
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ProductModel> Products { get; set; }
    }
}
