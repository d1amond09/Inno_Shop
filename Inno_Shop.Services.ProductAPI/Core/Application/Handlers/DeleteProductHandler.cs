using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

internal sealed class DeleteProductHandler(IProductRepository rep) : IRequestHandler<DeleteProductCommand>
{
	private readonly IProductRepository _rep = rep;

	public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _rep.GetProductByIdAsync(request.Id, request.TrackChanges)
			?? throw new ProductNotFoundException(request.Id);

		_rep.DeleteProduct(product);
		await _rep.SaveAsync();
	}
}
