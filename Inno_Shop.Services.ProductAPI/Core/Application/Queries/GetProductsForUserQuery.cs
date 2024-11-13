using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Queries;

public sealed record GetProductsForUserQuery(string? UserIdString, LinkParameters LinkParameters, bool TrackChanges) : 
	IRequest<ApiBaseResponse>;

