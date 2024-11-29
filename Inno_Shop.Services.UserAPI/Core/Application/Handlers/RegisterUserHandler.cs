using System;
using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public sealed class RegisterUserCommandHandler
	(IMapper mapper, UserManager<User> userManager) : 
	IRequestHandler<RegisterUserCommand, ApiBaseResponse>
{
	private readonly IMapper _mapper = mapper;
	private readonly UserManager<User> _userManager = userManager;

	public async Task<ApiBaseResponse> Handle(
		RegisterUserCommand request, 
		CancellationToken cancellationToken)
	{
		var user = _mapper.Map<User>(request.UserForRegistrationDto);
		var identityResult = await _userManager
			.CreateAsync(user, request.UserForRegistrationDto.Password!);
		
		if (identityResult.Succeeded)
			await _userManager
				.AddToRolesAsync(user, ["User"]);

        (IdentityResult, User) result = new (identityResult, user);

        return new ApiOkResponse<(IdentityResult, User)>(result);
	}
}
