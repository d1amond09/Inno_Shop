using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Commands;

public sealed record CreateProductCommand(ProductForCreationDto Product) : 
	IRequest<ProductDto>;
