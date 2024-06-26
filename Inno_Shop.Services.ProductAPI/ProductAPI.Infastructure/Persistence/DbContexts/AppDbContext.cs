using Inno_Shop.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.ProductAPI.DbContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
