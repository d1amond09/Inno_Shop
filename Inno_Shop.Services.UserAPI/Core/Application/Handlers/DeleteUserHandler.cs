using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

public sealed class DeleteUserHandler(UserManager<User> userManager) : 
	IRequestHandler<DeleteUserCommand, ApiBaseResponse>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<ApiBaseResponse> Handle(
		DeleteUserCommand request, 
		CancellationToken cancellationToken)
	{
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
		
		if(user is null)
			return new UserNotFoundResponse(request.Id);

        await _userManager.DeleteAsync(user);

		return new ApiOkResponse<User>(user);
	}
}
