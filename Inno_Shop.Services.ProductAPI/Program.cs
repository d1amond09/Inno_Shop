using System.Text;
using AspNetCoreRateLimit;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Service;
using Inno_Shop.Services.ProductAPI.Core.Application.Utility;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Presentation.ActionFilters;
using Inno_Shop.Services.ProductAPI.Presentation.Extensions;
using Inno_Shop.Services.UserAPI.Core.Domain.ConfigurationModels;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace Inno_Shop.Services.ProductAPI;

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
                s.SwaggerEndpoint("./v1/swagger.json", "Product API v1");
            });
        }

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
		s.AddMemoryCache();
        s.AddEndpointsApiExplorer();
        s.ConfigureSwagger();
        s.AddJwtConfiguration(c);

        s.AddScoped<IDataShaper<ProductDto>, DataShaper<ProductDto>>();
        s.AddScoped<IProductLinks, ProductLinks>();
        s.AddScoped<ValidateMediaTypeAttribute>();

        s.ConfigureRateLimitingOptions();
		s.AddHttpContextAccessor();

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
			config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
		}).AddNewtonsoftJson()
		.AddCustomCSVFormatter()
		.AddXmlDataContractSerializerFormatters();

        s.AddCustomMediaTypes();

        var jwtConfiguration = new JwtConfiguration();
        c.Bind(jwtConfiguration.Section, jwtConfiguration);
        var secretKey = Environment.GetEnvironmentVariable("SECRET");

        s.AddAuthentication("Bearer").AddJwtBearer("Bearer", opt =>
		{
			opt.Authority = "https://localhost:7243";
			opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtConfiguration.ValidIssuer,
                ValidAudience = jwtConfiguration.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
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

    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
		new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
		.Services.BuildServiceProvider()
		.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
		.OfType<NewtonsoftJsonPatchInputFormatter>().First();
}
