using AutoMapper;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;

namespace Inno_Shop.Services.ProductAPI.Presentation;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Product, ProductDto>();
		CreateMap<ProductForUpdateDto, Product>();
		CreateMap<ProductForCreationDto, Product>();
	}
}
