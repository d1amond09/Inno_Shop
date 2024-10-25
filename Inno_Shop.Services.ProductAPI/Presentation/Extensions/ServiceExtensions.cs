using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Service;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence;
using Inno_Shop.Services.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop.Services.ProductAPI.Presentation.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureCors(this IServiceCollection services) =>
		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
		});

	public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
		services.AddDbContext<AppDbContext>(opts =>
			opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b =>
			{
				b.MigrationsAssembly("Inno_Shop.Services.ProductAPI");
				b.EnableRetryOnFailure();
			})
		);

	public static void ConfigureProductRepository(this IServiceCollection services) =>
		services.AddScoped<IProductRepository, ProductRepository>();

	public static void ConfigureProductService(this IServiceCollection services) =>
		services.AddScoped<IProductService, ProductService>();
}
