using AutoMapper;
using Inno_Shop.Services.ProductAPI.Models;
using Inno_Shop.Services.ProductAPI.Models.DataTransferObjects;

namespace Inno_Shop.Services.ProductAPI.ProductAPI.Presentation;

public class MappingConfig
{
	public static MapperConfiguration RegisterMaps()
	{
		var mappingConfig = new MapperConfiguration(config =>
		{
			config.CreateMap<ProductDto, Product>();
			config.CreateMap<Product, ProductDto>();

		});

		return mappingConfig;
	}
}
