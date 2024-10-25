using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

internal sealed class UpdateProductHandler(IProductRepository rep, IMapper mapper) : IRequestHandler<UpdateProductCommand>
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
	{
		var productEntity = await _rep.GetProductByIdAsync(request.Id, request.TrackChanges)
			?? throw new ProductNotFoundException(request.Id);

		_mapper.Map(request.Product, productEntity);
		await _rep.SaveAsync();
	}
}
