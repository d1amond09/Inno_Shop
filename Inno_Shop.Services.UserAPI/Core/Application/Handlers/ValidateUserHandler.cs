using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public class ValidateUserCommandHandler(UserManager<User> userManager) : IRequestHandler<ValidateUserCommand, bool>
{
	private readonly UserManager<User> _userManager = userManager;

	public async Task<bool> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByNameAsync(request.UserForAuth.UserName);
		return (user != null && await _userManager.CheckPasswordAsync(user, request.UserForAuth.Password));
	}
}

