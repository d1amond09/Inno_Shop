using Inno_Shop.Services.UserAPI.Presentation.Controllers;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.UserAPI.Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(ISender sender) : ApiControllerBase
{
	private readonly ISender _sender = sender;

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
}

