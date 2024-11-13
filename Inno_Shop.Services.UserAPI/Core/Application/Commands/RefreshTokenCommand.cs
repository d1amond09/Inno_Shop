using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;

namespace Inno_Shop.Services.UserAPI.Core.Application.Commands;

public sealed record RefreshTokenCommand(TokenDto TokenDto) : 
    IRequest<ApiBaseResponse>;