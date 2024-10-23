using AutoMapper;
using Inno_Shop.Services.ProductAPI.Models;
using Inno_Shop.Services.ProductAPI.Models.DataTransferObjects;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Inno_Shop.Services.ProductAPI.ProductAPI.Presentation;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Product, ProductDto>();
	}
}
