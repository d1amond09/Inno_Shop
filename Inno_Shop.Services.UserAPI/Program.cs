using Inno_Shop.Services.UserAPI.Presentation.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.UserAPI;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		ConfigureServices(builder.Services, builder.Configuration);
		builder.Services.AddControllers();

		var app = builder.Build();


		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}

	public static void ConfigureServices(IServiceCollection s, IConfiguration c)
	{
		s.AddAuthentication();
		s.ConfigureIdentity();
		s.AddHttpContextAccessor();
		s.ConfigureSqlContext(c);

		s.AddControllers(config =>
		{
			config.RespectBrowserAcceptHeader = true;
			config.ReturnHttpNotAcceptable = true;
		});
	}

	public static void ConfigureApp(IApplicationBuilder app)
	{
		app.UseCors("CorsPolicy");
		app.UseStaticFiles();
		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});
		app.UseAuthentication();
		app.UseAuthorization();
	}
}
