using System.ComponentModel.Design;
using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Service;

public class ProductService(IProductRepository rep, IMapper mapper) : IProductService
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ProductDto> CreateProductAsync(ProductForCreationDto product)
	{
		var productToCreate = _mapper.Map<Product>(product);
		_rep.CreateProduct(productToCreate);

		await _rep.SaveAsync();

		var productToReturn = _mapper.Map<ProductDto>(productToCreate);
		return productToReturn;
	}

	public async Task DeleteProductAsync(Guid productId, bool trackChanges)
	{
		var product = await _rep.GetProductByIdAsync(productId, trackChanges) 
			?? throw new ProductNotFoundException(productId);

		_rep.DeleteProduct(product);
		await _rep.SaveAsync();
	}
	public async Task UpdateProductAsync(Guid productId, ProductForUpdateDto productForUpdate, bool trackChanges)
	{
		var productEntity = await _rep.GetProductByIdAsync(productId, trackChanges) 
			?? throw new ProductNotFoundException(productId);

		_mapper.Map(productForUpdate, productEntity);
		await _rep.SaveAsync();
	}

	public async Task<ApiBaseResponse> GetProductByIdAsync(Guid productId, bool trackChanges)
	{
		var product = await _rep.GetProductByIdAsync(productId, trackChanges);
		if (product is null)
			return new ProductNotFoundResponse(productId);

		var productDto = _mapper.Map<ProductDto>(product);
		return new ApiOkResponse<ProductDto>(productDto);
	}

	public async Task<ApiBaseResponse> GetProductsAsync(bool trackChanges)
	{
		var products = await _rep.GetProductsAsync(trackChanges);
		var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
		return new ApiOkResponse<IEnumerable<ProductDto>>(productsDto);
	}

}
