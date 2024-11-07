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

namespace Inno_Shop.Services.ProductAPI.Presentation.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[Consumes("application/json")]
[Route("api/products")]
[ApiController]
public class ProductController(ISender sender) : ApiControllerBase
{
	private readonly ISender _sender = sender;

	[HttpGet]
	public async Task<IActionResult> GetProducts([FromQuery] ProductParameters productParameters)
	{
		var baseResult = await _sender.Send(new GetProductsQuery(productParameters, TrackChanges: false));
		var (products, metaData) = baseResult.GetResult<(IEnumerable<ProductDto> products, MetaData metaData)>();

		Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
		return Ok(products);
	}

    [Authorize]
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
    [HttpPost]
	public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto product)
	{
		if (product is null)
			return BadRequest("ProductForCreationDto object is null");

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);
		
		var createdProduct = await _sender.Send(new CreateProductCommand(product));

		return CreatedAtRoute("ProductById", new { id = createdProduct.ProductID }, createdProduct);
	}

    [Authorize]
    [HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteProduct(Guid id)
	{
		var baseResult = await _sender.Send(new DeleteProductCommand(id, TrackChanges: false));
		
		if (!baseResult.Success)
			return ProcessError(baseResult);

		return NoContent();
	}

    [Authorize]
    [HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductForUpdateDto product)
	{
		if (product is null)
			return BadRequest("ProductForUpdateDto object is null");

		// сделать валидацию в Application -> Validators


		var baseResult = await _sender.Send(new UpdateProductCommand(id, product, TrackChanges: true));

		if (!baseResult.Success)
			return ProcessError(baseResult);

		return NoContent();
	}
}
