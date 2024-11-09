using Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.UserAPI.Presentation.Controllers;

[Route("api")]
[ApiController]
public class RootController(LinkGenerator linkGenerator) : ControllerBase
{
    private readonly LinkGenerator _linkGenerator =
    linkGenerator;

    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
    {
        if (mediaType.Contains("application/apiroot"))
        {
            List<Link> list =
            [
                new Link {
                    Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new{}),
                    Rel = "self",
                    Method = "GET"
                },
                new Link {
                    Href = _linkGenerator.GetUriByName(HttpContext, "GetUsers", new{}),
                    Rel = "users",
                    Method = "GET"
                },
                new Link {
                    Href = _linkGenerator.GetUriByName(HttpContext, "SignUp", new{}),
                    Rel = "sign_up",
                    Method = "POST"
                },
                new Link {
                    Href = _linkGenerator.GetUriByName(HttpContext, "SignIn", new{}),
                    Rel = "sign_in",
                    Method = "POST"
                }
            ];

            return Ok(list);
        }

        return NoContent();
    }
}
