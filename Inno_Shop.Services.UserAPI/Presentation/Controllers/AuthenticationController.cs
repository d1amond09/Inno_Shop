using Inno_Shop.Services.UserAPI.Presentation.Controllers;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Inno_Shop.Services.UserAPI.Presentation.Extensions;

namespace Inno_Shop.Services.UserAPI.Presentation.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[Consumes("application/json")]
[Route("api/authentication")]
[ApiController]
public class AuthenticationController(ISender sender, UserManager<User> userManager) : ApiControllerBase
{
	private readonly ISender _sender = sender;
	private readonly UserManager<User> _userManager = userManager;

	[HttpPost(Name = "SignUp")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
	{
        var baseResult = await _sender.Send(new RegisterUserCommand(userForRegistration));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        var result = baseResult.GetResult<IdentityResult>();

		if (!result.Succeeded)
		{
			foreach (var error in result.Errors)
			{
				ModelState.TryAddModelError(error.Code, error.Description);
			}
			return BadRequest(ModelState);
		}
		return StatusCode(201);
	}

	[HttpPost("login", Name = "SignIn")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuth)
	{
		var baseResult = await _sender.Send(new ValidateUserCommand(userForAuth));

        if (!baseResult.Success)
            return ProcessError(baseResult);

		var isValidUser = baseResult.GetResult<bool>();

        if (!isValidUser)
			return Unauthorized("Invalid username or password.");

		var user = await _userManager.FindByNameAsync(userForAuth.UserName!);

		if (user == null)
			return Unauthorized("Invalid username or password.");

		var tokenDtoBaseResult = await _sender.Send(new CreateTokenCommand(user, PopulateExp: true));

        if (!tokenDtoBaseResult.Success)
            return ProcessError(tokenDtoBaseResult);

		var tokenDto = tokenDtoBaseResult.GetResult<TokenDto>();

        return Ok(tokenDto);
	}
}

