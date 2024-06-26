using Microsoft.Extensions.Configuration;
using Inno_Shop.Services.ProductAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using AutoMapper;
using Inno_Shop.Services.ProductAPI.ProductAPI.Presentation;

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
			app.UseSwaggerUI();
		}

		ConfigureApp(app);

		app.MapControllers();

		app.Run();
	}
	public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		services.AddControllers();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		services.AddDbContext<AppDbContext>(op =>
			op.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

		IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
		services.AddSingleton(mapper);
		services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
	}

	public static void ConfigureApp(IApplicationBuilder app)
	{
		app.UseHttpsRedirection();
		app.UseAuthorization();
	}
}
