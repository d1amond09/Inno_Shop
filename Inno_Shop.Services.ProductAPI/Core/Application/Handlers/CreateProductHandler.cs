using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

internal sealed class CreateCompanyHandler(IProductRepository rep, IMapper mapper) : IRequestHandler<CreateProductCommand, ProductDto>
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		var productToCreate = _mapper.Map<Product>(request.Product);
		_rep.CreateProduct(productToCreate);
		await _rep.SaveAsync();
		var productToReturn = _mapper.Map<ProductDto>(productToCreate);
		return productToReturn;
	}
}
