using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;

namespace Inno_Shop.Services.UserAPI.Core.Application.Commands;

public sealed record UpdateUserCommand (Guid Id, UserForUpdateDto User) : 
    IRequest<ApiBaseResponse>;

