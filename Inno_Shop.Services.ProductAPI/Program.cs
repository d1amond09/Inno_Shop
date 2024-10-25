using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Service;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence;
using Inno_Shop.Services.ProductAPI.Presentation;
using Inno_Shop.Services.ProductAPI.Presentation.Extensions;
using Inno_Shop.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Inno_Shop.Services.ProductAPI;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		ConfigureServices(builder.Services, builder.Configuration);

		var app = builder.Build();
		app.ConfigureExceptionHandler();
		if (app.Environment.IsProduction())
			app.UseHsts();

		ConfigureApp(app);

		app.MapControllers();

		app.Run();
	}

	public static void ConfigureServices(IServiceCollection s, IConfiguration c)
	{
		s.ConfigureCors();
		s.ConfigureProductRepository();
		s.ConfigureProductService();
		s.ConfigureSqlContext(c);
		s.ConfigureAutoMapper();
		s.AddControllers(config =>
		{
			config.RespectBrowserAcceptHeader = true;
			config.ReturnHttpNotAcceptable = true;
		});
	}

	public static void ConfigureApp(IApplicationBuilder app)
	{
		app.UseCors("CorsPolicy");
		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});
		app.UseAuthorization();
	}
}
