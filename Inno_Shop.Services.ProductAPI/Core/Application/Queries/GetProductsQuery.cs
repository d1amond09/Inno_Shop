using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Queries;

public sealed record GetProductsQuery(bool TrackChanges) : 
	IRequest<ApiBaseResponse>;

