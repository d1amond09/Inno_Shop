using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Service;

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
