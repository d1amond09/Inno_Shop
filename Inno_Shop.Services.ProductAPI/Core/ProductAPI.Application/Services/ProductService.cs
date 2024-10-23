using AutoMapper;
using Inno_Shop.Services.ProductAPI.Models.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Interfaces;

namespace Inno_Shop.Services.ProductAPI.ProductAPI.Core.ProductAPI.Application.Services;

public class ProductService(IProductRepository rep, IMapper mapper) : IProductService
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<IEnumerable<ProductDto>> GetProducts(bool trackChanges)
	{
		try
		{
			var products = await _rep.GetProducts(trackChanges);
			var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
			return productsDto;
		}
		catch (Exception ex)
		{
			//_logger.LogError($"Something went wrong in the { nameof(GetProducts)} service method {ex}");
			throw;
		}
	}
}
