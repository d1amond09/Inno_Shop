using AspNetCoreRateLimit;
using FluentValidation;
using Inno_Shop.Services.ProductAPI.Core.Application;
using Inno_Shop.Services.ProductAPI.Core.Application.Behaviors;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Infastructure.Persistence;
using Inno_Shop.Services.ProductAPI.Presentation.GlobalException;
using Inno_Shop.Services.ProductAPI.Repository;
using Inno_Shop.Services.UserAPI.Core.Domain.ConfigurationModels;
using Marvin.Cache.Headers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Inno_Shop.Services.ProductAPI.Presentation.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureCors(this IServiceCollection services) =>
		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader()
			.WithExposedHeaders("X-Pagination"));
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

	public static void ConfigureAutoMapper(this IServiceCollection services) =>
		services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

	public static void ConfigureMediatR(this IServiceCollection services) =>
		services.AddMediatR(cfg => 
			cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

	public static void ConfigureFluentValidation(this IServiceCollection services)
	{
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
	}

	public static void ConfigureExceptionHandler(this IServiceCollection services) =>
		services.AddExceptionHandler<GlobalExceptionHandler>();

	public static void ConfigureResponseCaching(this IServiceCollection services) =>
		services.AddResponseCaching();

	public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
		services.AddHttpCacheHeaders(
			(expirationOpt) => {
				expirationOpt.MaxAge = 120;
				expirationOpt.CacheLocation = CacheLocation.Private;
			},
			(validationOpt) => {
				validationOpt.MustRevalidate = true;
			}
		);

	public static void ConfigureRateLimitingOptions(this IServiceCollection services)
	{
		List<RateLimitRule> rateLimitRules = [
			new() {
				Endpoint = "*",
				Limit = 10,
				Period = "1s"
			}
		];

		services.Configure<IpRateLimitOptions>(opt => {
			opt.GeneralRules = rateLimitRules;
		});

		services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
		services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
		services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
		services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
	}

    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>("JwtSettings", configuration.GetSection("JwtSettings"));
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Product API",
                Version = "v1"
            });
            s.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
			s.EnableAnnotations();
            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            s.AddSecurityRequirement(new OpenApiSecurityRequirement() { {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Name = "Bearer",
                },
                new List<string>()
            } });
        });
    }

}
