using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Application.Commands;

public sealed record ValidateUserCommand(UserForAuthenticationDto UserForAuth) :
	IRequest<ApiBaseResponse>;

