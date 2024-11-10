using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

public sealed class CreateProductHandler(IProductRepository rep, IMapper mapper) : IRequestHandler<CreateProductCommand, ApiBaseResponse>
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
        if (!Guid.TryParse(request.UserIdString, out Guid userId))
            return new ApiInvalidUserIdBadRequestResponse(request.UserIdString);

        var productToCreate = _mapper.Map<Product>(request.Product);
        productToCreate.UserID = userId;

        _rep.CreateProduct(productToCreate);
		await _rep.SaveAsync();

		var productToReturn = _mapper.Map<ProductDto>(productToCreate);
        return new ApiOkResponse<ProductDto>(productToReturn);
	}
}
