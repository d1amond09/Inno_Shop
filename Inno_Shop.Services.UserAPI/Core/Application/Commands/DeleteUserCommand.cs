using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;

namespace Inno_Shop.Services.UserAPI.Core.Application.Commands;

public sealed record DeleteUserCommand(Guid Id) : 
    IRequest<ApiBaseResponse>;

