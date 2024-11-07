using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Commands;

public sealed record UpdateProductCommand
	(Guid Id, ProductForUpdateDto Product, bool TrackChanges) : IRequest<ApiBaseResponse>;

