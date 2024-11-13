using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

public sealed class UpdateProductHandler(IProductRepository rep, IMapper mapper) : IRequestHandler<UpdateProductCommand, ApiBaseResponse>
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
	{
        if (!Guid.TryParse(request.UserIdString, out Guid userId))
            return new ApiInvalidUserIdBadRequestResponse(request.UserIdString);

        var productEntity = await _rep.GetProductByIdAsync(request.Id, request.TrackChanges);

		if (productEntity is null)
			return new ProductNotFoundResponse(request.Id);

        if (productEntity.UserID != userId)
            return new ApiProductNotBelongUserBadRequestResponse(request.Id, userId);

        _mapper.Map(request.Product, productEntity);
		await _rep.SaveAsync();
		return new ApiOkResponse<Product>(productEntity);
	}
}
