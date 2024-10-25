using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;

namespace Inno_Shop.Services.ProductAPI.Presentation.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductService service) : ControllerBase
{
	private readonly IProductService _service = service;

	[HttpGet]
	public async Task<IActionResult> GetProducts()
	{
		var products = await _service.GetProductsAsync(trackChanges: false);
		return Ok(products);
	}

	[HttpGet("{id:guid}", Name = "ProductById")]
	public async Task<IActionResult> GetProduct(Guid id)
	{
		var product = await _service.GetProductByIdAsync(id, trackChanges: false);
		return Ok(product);
	}

	[HttpPost]
	public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto product)
	{
		if (product is null)
			return BadRequest("ProductForCreationDto object is null");

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		var createdProduct = await _service.CreateProductAsync(product);

		return CreatedAtRoute("ProductById", new { id = createdProduct.Id }, createdProduct);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteProduct(Guid id)
	{
		await _service.DeleteProductAsync(id, trackChanges: false);

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductForUpdateDto product)
	{
		if (product is null)
			return BadRequest("ProductForUpdateDto object is null");

		await _service.UpdateProductAsync(id, product, trackChanges: true);

		return NoContent();
	}
}
