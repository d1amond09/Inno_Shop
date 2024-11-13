using Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;

namespace Inno_Shop.Services.UserAPI.Core.Application.Queries;

public sealed record GetUsersQuery(LinkParameters LinkParameters) : 
	IRequest<ApiBaseResponse>;

