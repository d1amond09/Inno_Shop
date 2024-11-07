﻿using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

internal sealed class DeleteProductHandler(IProductRepository rep) : IRequestHandler<DeleteProductCommand, ApiBaseResponse>
{
	private readonly IProductRepository _rep = rep;

	public async Task<ApiBaseResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _rep.GetProductByIdAsync(request.Id, request.TrackChanges);
		
		if(product is null)
			return new ProductNotFoundResponse(request.Id);

		_rep.DeleteProduct(product);
		await _rep.SaveAsync();

		return new ApiOkResponse<Product>(product);
	}
}
