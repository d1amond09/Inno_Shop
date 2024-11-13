using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public class ValidateUserCommandHandler(UserManager<User> userManager) : 
	IRequestHandler<ValidateUserCommand, ApiBaseResponse>
{
	private readonly UserManager<User> _userManager = userManager;

	public async Task<ApiBaseResponse> Handle(
		ValidateUserCommand request, 
		CancellationToken cancellationToken)
	{
		var user = await _userManager
			.FindByNameAsync(request.UserForAuth.UserName!);
		
		bool result = (user != null && await _userManager
			.CheckPasswordAsync(user, request.UserForAuth.Password!));
		
		return new ApiOkResponse<bool>(result);
	}
}

