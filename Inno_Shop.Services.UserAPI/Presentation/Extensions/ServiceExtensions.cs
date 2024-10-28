﻿using Inno_Shop.Services.UserAPI.Core.Application;
using Inno_Shop.Services.UserAPI.Presentation;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Inno_Shop.Services.UserAPI.Presentation.GlobalException;

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
		})
		.AddEntityFrameworkStores<UserDbContext>()
		.AddDefaultTokenProviders();
	}

	public static void ConfigureAutoMapper(this IServiceCollection services) =>
		services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

	public static void ConfigureMediatR(this IServiceCollection services) =>
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

	public static void ConfigureExceptionHandler(this IServiceCollection services) =>
		services.AddExceptionHandler<GlobalExceptionHandler>();
}