using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Presentation.Extensions;
using MediatR;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;
using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;
using System.Text.Json;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using Inno_Shop.Services.ProductAPI.Presentation.ActionFilters;
using Inno_Shop.Services.ProductAPI.Core.Domain.Models;
using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using System.Security.Claims;

namespace Inno_Shop.Services.ProductAPI.Presentation.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[Consumes("application/json")]
[Route("api/products")]
[ApiController]
public class ProductController(ISender sender) : ApiControllerBase
{
	private readonly ISender _sender = sender;

	[HttpGet("all", Name = "GetProducts")]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetProducts([FromQuery] ProductParameters productParameters)
	{
		var linkParams = new LinkParameters(productParameters, HttpContext);
		var baseResult = await _sender.Send(new GetProductsQuery(linkParams, TrackChanges: false));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        var (linkResponse, metaData) = baseResult.GetResult<(LinkResponse, MetaData)>();

		Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
        
		return linkResponse.HasLinks ? 
			Ok(linkResponse.LinkedEntities) : 
			Ok(linkResponse.ShapedEntities);
	}

    [HttpGet("{id:guid}", Name = "ProductById")]
	public async Task<IActionResult> GetProduct(Guid id)
	{
		var baseResult = await _sender.Send(new GetProductQuery(id, TrackChanges: false));

		if (!baseResult.Success)
			return ProcessError(baseResult);

		var products = baseResult.GetResult<ProductDto>();
		return Ok(products);
	}

    [Authorize]
    [HttpGet(Name = "GetProductsForUser")]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetProductsForUser([FromQuery] ProductParameters productParameters)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var linkParams = new LinkParameters(productParameters, HttpContext);
        var baseResult = await _sender.Send(new GetProductsForUserQuery(userIdString, linkParams, TrackChanges: false));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        var (linkResponse, metaData) = baseResult.GetResult<(LinkResponse, MetaData)>();

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));

        return linkResponse.HasLinks ?
            Ok(linkResponse.LinkedEntities) :
            Ok(linkResponse.ShapedEntities);
    }

    [Authorize]
    [HttpPost(Name = "CreateProduct")]
	public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto product)
	{
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var baseResult = await _sender.Send(new CreateProductCommand(userIdString, product));

        if (!baseResult.Success)
            return ProcessError(baseResult);

		var createdProduct = baseResult.GetResult<ProductDto>();

        return CreatedAtRoute("ProductById", new { id = createdProduct.ProductID }, createdProduct);
	}

    [Authorize]
    [HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteProduct(Guid id)
	{
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var baseResult = await _sender.Send(new DeleteProductCommand(userIdString, id, TrackChanges: false));
		
		if (!baseResult.Success)
			return ProcessError(baseResult);

		return NoContent();
	}

    [Authorize]
    [HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductForUpdateDto product)
	{
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var baseResult = await _sender.Send(new UpdateProductCommand(userIdString, id, product, TrackChanges: true));

		if (!baseResult.Success)
			return ProcessError(baseResult);

		return NoContent();
	}
}
