using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Presentation.Extensions;
using MediatR;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;

namespace Inno_Shop.Services.ProductAPI.Presentation.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(ISender sender) : ApiControllerBase
{
	private readonly ISender _sender = sender;

	[HttpGet]
	public async Task<IActionResult> GetProducts()
	{
		var baseResult = await _sender.Send(new GetProductsQuery(TrackChanges: false));
		var products = baseResult.GetResult<IEnumerable<ProductDto>>();
		return Ok(products);

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

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteProduct(Guid id)
	{
		await _sender.Send(new DeleteProductCommand(id, TrackChanges: false));

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductForUpdateDto product)
	{
		if (product is null)
			return BadRequest("ProductForUpdateDto object is null");

		await _sender.Send(new UpdateProductCommand(id, product, TrackChanges: true));

		return NoContent();
	}
}
