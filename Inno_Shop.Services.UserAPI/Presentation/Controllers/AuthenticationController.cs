using Inno_Shop.Services.UserAPI.Presentation.Controllers;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.Services.UserAPI.Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(ISender sender, UserManager<User> userManager) : ApiControllerBase
{
	private readonly ISender _sender = sender;
	private readonly UserManager<User> _userManager = userManager;

	[HttpPost]
	public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
	{
		var result = await _sender.Send(new RegisterUserCommand(userForRegistration));

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

	[HttpPost("login")]
	public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuth)
	{
		var isValidUser = await _sender.Send(new ValidateUserCommand(userForAuth));

		if (!isValidUser)
		{
			return Unauthorized("Invalid username or password.");
		}

		var user = await _userManager.FindByNameAsync(userForAuth.UserName);
		if (user == null)
		{
			return Unauthorized("Invalid username or password.");
		}
		var token = await _sender.Send(new CreateTokenCommand(user));

		return Ok(new { Token = token });
	}

	public async Task<string> CreateToken(User user)
	{
		var command = new CreateTokenCommand(user);
		return await _sender.Send(command);
	}
}

