using Inno_Shop.Services.ProductAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.ProductAPI.Infastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Product> Products { get; set; }
}
