using Inno_Shop.Services.ProductAPI.Presentation.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.ProductAPI;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		ConfigureServices(builder.Services, builder.Configuration);

		var app = builder.Build();

		if (app.Environment.IsProduction())
			app.UseHsts();

		ConfigureApp(app);

		app.MapControllers();

		app.Run();
	}
	 
	public static void ConfigureServices(IServiceCollection s, IConfiguration c)
	{
		s.ConfigureExceptionHandler();
		s.AddProblemDetails();
		s.ConfigureCors();
		s.ConfigureProductRepository();
		s.ConfigureSqlContext(c);
		s.ConfigureMediatR(); 
		s.ConfigureAutoMapper();
		s.ConfigureFluentValidation();
		s.ConfigureResponseCaching();
		s.ConfigureHttpCacheHeaders();

		s.Configure<ApiBehaviorOptions>(options =>
		{
			options.SuppressModelStateInvalidFilter = true;
		});

		s.AddControllers(config =>
		{
			config.RespectBrowserAcceptHeader = true;
			config.ReturnHttpNotAcceptable = true;
			config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
			{
				Duration = 120
			});
		});
	}

	public static void ConfigureApp(IApplicationBuilder app)
	{
		app.UseExceptionHandler();
		app.UseCors("CorsPolicy");
		app.UseResponseCaching();
		app.UseHttpCacheHeaders();
		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});
		app.UseAuthorization();
	}
}
