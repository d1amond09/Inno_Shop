using AutoMapper;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Core.Application.Handlers;

internal sealed class UpdateUserHandler(UserManager<User> userManager, IMapper mapper) : IRequestHandler<UpdateUserCommand, ApiBaseResponse>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
        var userEntity = await _userManager.FindByIdAsync(request.Id.ToString());

		if (userEntity is null)
			return new UserNotFoundResponse(request.Id);

        _mapper.Map(request.User, userEntity);

		await _userManager.UpdateAsync(userEntity);

        return new ApiOkResponse<User>(userEntity);
	}
}
