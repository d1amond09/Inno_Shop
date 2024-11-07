using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

internal sealed class RegisterUserCommandHandler(IMapper mapper, UserManager<User> userManager) : 
	IRequestHandler<RegisterUserCommand, IdentityResult>
{
	private readonly IMapper _mapper = mapper;
	private readonly UserManager<User> _userManager = userManager;

	public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var user = _mapper.Map<User>(request.UserForRegistrationDto);
		var result = await _userManager.CreateAsync(user, request.UserForRegistrationDto.Password);
		if (result.Succeeded)
			await _userManager.AddToRolesAsync(user, request.UserForRegistrationDto.Roles);
		return result;
	}
}
