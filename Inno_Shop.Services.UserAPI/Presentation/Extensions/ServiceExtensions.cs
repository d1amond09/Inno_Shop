using Inno_Shop.Services.UserAPI.Core.Application;
using Inno_Shop.Services.UserAPI.Presentation;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Inno_Shop.Services.UserAPI.Presentation.GlobalException;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Inno_Shop.Services.UserAPI.Core.Domain.ConfigurationModels;
using Microsoft.OpenApi.Models;
using Inno_Shop.Services.UserAPI.Core.Application.Contracts;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Application.Service;
using Inno_Shop.Services.UserAPI.Core.Application.Utility;
using Inno_Shop.Services.UserAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Marvin.Cache.Headers;
using AspNetCoreRateLimit;
using Inno_Shop.Services.UserAPI.Presentation.CustomTokenProviders;
using Microsoft.AspNetCore.Http.Features;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Inno_Shop.Services.UserAPI.Presentation.Extensions;

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
        services.AddDbContext<UserDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b =>
            {
                b.MigrationsAssembly("Inno_Shop.Services.UserAPI");
                b.EnableRetryOnFailure();
            })
        );

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = true;
            o.Password.RequireUppercase = true;
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequiredLength = 8;
            o.User.RequireUniqueEmail = true;
            o.SignIn.RequireConfirmedEmail = true;
            o.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
        })
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");

        services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(2));

        services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromDays(3));
    }

    public static void ConfigureAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

    public static void ConfigureMediatR(this IServiceCollection services) =>
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

    public static void ConfigureExceptionHandler(this IServiceCollection services) =>
        services.AddExceptionHandler<GlobalExceptionHandler>();

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration();
        configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtConfiguration.ValidIssuer,
                ValidAudience = jwtConfiguration.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        configuration.GetValue<string>("SECRET")!
                ))
            };
        });
    }

    public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();

    public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
        services.AddHttpCacheHeaders(
            (expirationOpt) =>
            {
                expirationOpt.MaxAge = 120;
                expirationOpt.CacheLocation = CacheLocation.Private;
            },
            (validationOpt) =>
            {
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

        services.Configure<IpRateLimitOptions>(opt =>
        {
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
        services.Configure<JwtConfiguration>("JwtAPI2Settings", configuration.GetSection("JwtAPI2Settings"));
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "User API",
                Version = "v1"
            });
            s.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

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

    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var newtonsoftJsonOutputFormatter = config.OutputFormatters
                .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

            newtonsoftJsonOutputFormatter?.SupportedMediaTypes
                .Add("application/hateoas+json");

            newtonsoftJsonOutputFormatter?.SupportedMediaTypes
                .Add("application/apiroot+json");

            var xmlOutputFormatter = config.OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

            xmlOutputFormatter?.SupportedMediaTypes
                .Add("application/hateoas+xml");

            xmlOutputFormatter?.SupportedMediaTypes
                .Add("application/apiroot+xml");
        });
    }

    public static void ConfigureDataShaping(this IServiceCollection services) =>
       services.AddScoped<IDataShaper<UserDto>, DataShaper<UserDto>>();

    public static void ConfigureHATEOAS(this IServiceCollection services)
    {
        services.AddScoped<IUserLinks, UserLinks>();
        services.AddScoped<ValidateMediaTypeAttribute>();
    }

    public static void ConfigureEmailSending(this IServiceCollection services, IConfiguration config)
    {
        EmailConfiguration? emailConfig = config
            .GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();
        ArgumentNullException.ThrowIfNull(emailConfig);
        services.AddSingleton(emailConfig);

        services.AddScoped<IEmailSender, EmailSender>();

        services.Configure<FormOptions>(o => {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = int.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });
    }
}