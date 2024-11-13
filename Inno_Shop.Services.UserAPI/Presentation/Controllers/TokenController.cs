using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inno_Shop.Services.UserAPI.Presentation.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[Consumes("application/json")]
[Route("api/token")]
[ApiController]
public class TokenController(ISender sender) : ApiControllerBase
{
	private readonly ISender _sender = sender;

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var baseResult = await _sender.Send(new RefreshTokenCommand(tokenDto));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        var tokenDtoToReturn = baseResult.GetResult<TokenDto>();

        return Ok(tokenDtoToReturn);
    }
}
