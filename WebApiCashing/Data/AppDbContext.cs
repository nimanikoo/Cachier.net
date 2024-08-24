using Microsoft.EntityFrameworkCore;
using WebApiCaching.Models;

namespace WebApiCaching.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}