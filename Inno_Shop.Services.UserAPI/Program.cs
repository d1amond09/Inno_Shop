using Inno_Shop.Services.UserAPI.Presentation.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Inno_Shop.Services.UserAPI;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		ConfigureServices(builder.Services, builder.Configuration);

		var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("./v1/swagger.json", "User API v1");
            });
        }

        app.UseHttpsRedirection();

		app.MapControllers();

		app.Run();
	}

	public static void ConfigureServices(IServiceCollection s, IConfiguration config)
	{
		s.AddAuthentication();
		s.ConfigureIdentity();
		s.ConfigureJWT(config);
        s.AddJwtConfiguration(config);
        s.AddProblemDetails();
		s.ConfigureExceptionHandler();
		s.AddHttpContextAccessor();
		s.ConfigureSqlContext(config);
		s.ConfigureMediatR();
		s.ConfigureAutoMapper();
        s.AddEndpointsApiExplorer();
        s.ConfigureSwagger();

        s.AddControllers(cnfg =>
		{
			cnfg.RespectBrowserAcceptHeader = true;
            cnfg.ReturnHttpNotAcceptable = true;
		});
	}

	public static void ConfigureApp(IApplicationBuilder app)
	{
		app.UseExceptionHandler();
		app.UseCors("CorsPolicy");
		app.UseStaticFiles();
       
        app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});
		app.UseRouting();
		app.UseAuthentication();
		app.UseAuthorization();
    }
}
