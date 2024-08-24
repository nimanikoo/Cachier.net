using Cachier.net.Models;
using Microsoft.EntityFrameworkCore;

namespace Cachier.net.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}