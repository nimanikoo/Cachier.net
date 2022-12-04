using Microsoft.EntityFrameworkCore;
using WebApiCashing.Models;

namespace WebApiCashing.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Customer> Customers { get; set; }

    }
}