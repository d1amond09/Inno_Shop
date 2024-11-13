using System.Data;
using System.Numerics;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.ProductAPI.Infastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Product> Products { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfiguration(new ProductConfiguration());
	}

}
