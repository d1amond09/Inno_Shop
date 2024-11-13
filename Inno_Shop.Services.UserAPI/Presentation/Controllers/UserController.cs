using System.Security.Claims;
using System.Text.Json;
using Inno_Shop.Services.UserAPI.Presentation.ActionFilters;
using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Application.Queries;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Inno_Shop.Services.UserAPI.Core.Domain.RequestFeatures;
using Inno_Shop.Services.UserAPI.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.UserAPI.Presentation.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[Consumes("application/json")]
[Route("api/users")]
[ApiController]
public class UserController(ISender sender, UserManager<User> userManager) : ApiControllerBase
{
    private readonly ISender _sender = sender;
    private readonly UserManager<User> _userManager = userManager;

    [Authorize]
    [HttpGet(Name = "GetUsers")]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParameters)
    {
        var linkParams = new LinkParameters(userParameters, HttpContext);
        var baseResult = await _sender.Send(new GetUsersQuery(linkParams));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        var (linkResponse, metaData) = baseResult.GetResult<(LinkResponse, MetaData)>();

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));

        return linkResponse.HasLinks ?
            Ok(linkResponse.LinkedEntities) :
            Ok(linkResponse.ShapedEntities);
    }

    [Authorize]
    [HttpGet("{id:guid}", Name = "GetUser")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var baseResult = await _sender.Send(new GetUserQuery(id));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        var users = baseResult.GetResult<UserDto>();
        return Ok(users);
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("{id:guid}", Name = "DeleteUser")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var baseResult = await _sender.Send(new DeleteUserCommand(id));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        return NoContent();
    }

    [Authorize(Roles = "Administrator")]
    [HttpPut("{id:guid}", Name = "UpdateUser")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserForUpdateDto user)
    {
        var baseResult = await _sender.Send(new UpdateUserCommand(id, user));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        return NoContent();
    }
}
