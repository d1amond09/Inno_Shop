using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Application.Commands;

public sealed record CreateTokenCommand(User User) : IRequest<string>;

