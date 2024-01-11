using Customer.Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer.Microservice
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<CustomerModel> Customers { get; set; }
    }
}
