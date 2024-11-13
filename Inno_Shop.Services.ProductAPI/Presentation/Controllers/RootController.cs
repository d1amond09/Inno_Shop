using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.ProductAPI.Presentation.Controllers;

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
                    Href = _linkGenerator.GetUriByName(HttpContext, "GetProducts", new{}),
                    Rel = "products",
                    Method = "GET"
                },
                new Link {
                    Href = _linkGenerator.GetUriByName(HttpContext, "CreateProduct", new{}),
                    Rel = "create_product",
                    Method = "POST"
                }
            ];

            return Ok(list);
        }

        return NoContent();
    }
}
