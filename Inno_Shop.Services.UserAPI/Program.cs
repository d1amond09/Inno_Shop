using AspNetCoreRateLimit;
using Inno_Shop.Services.UserAPI.Core.Application.Contracts;
using Inno_Shop.Services.UserAPI.Core.Application.Service;
using Inno_Shop.Services.UserAPI.Core.Domain.ConfigurationModels;
using Inno_Shop.Services.UserAPI.Presentation.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        ConfigureApp(app);

        app.MapControllers();
        
        app.Run();
	}

	public static void ConfigureServices(IServiceCollection s, IConfiguration config)
	{
		s.ConfigureCors(config);
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

        s.ConfigureResponseCaching();
        s.ConfigureHttpCacheHeaders();
        s.AddMemoryCache();

        s.ConfigureRateLimitingOptions();

        s.AddEndpointsApiExplorer();
        s.ConfigureSwagger();

        s.ConfigureDataShaping();
        s.ConfigureHATEOAS();

        s.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        

        s.AddControllers(cnfg =>
        {
	        cnfg.RespectBrowserAcceptHeader = true;
	        cnfg.ReturnHttpNotAcceptable = true;
	        cnfg.CacheProfiles.Add("120SecondsDuration", new CacheProfile
	        {
		        Duration = 120
	        });
        }).AddNewtonsoftJson();

        s.AddCustomMediaTypes();
        s.ConfigureEmailSending(config);
        s.AddAuthorization();
    }

	public static void ConfigureApp(IApplicationBuilder app)
	{
		app.UseExceptionHandler();
        app.UseIpRateLimiting();
        app.UseCors("CorsPolicy");
        app.UseResponseCaching();
        app.UseHttpCacheHeaders();
        app.UseHttpsRedirection();
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
