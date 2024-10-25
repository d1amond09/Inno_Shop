using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Queries;

public sealed record GetProductQuery(Guid ProductId, bool TrackChanges) : 
	IRequest<ApiBaseResponse>;

