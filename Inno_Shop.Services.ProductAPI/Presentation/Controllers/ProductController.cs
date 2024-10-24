using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.Services.ProductAPI.Presentation.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductService service) : ControllerBase
{
	private readonly IProductService _service = service;

	[HttpGet]
	public async Task<IActionResult> GetProducts()
	{
		try
		{
			var products = await _service.GetProducts(trackChanges: false);
			return Ok(products);
		}
		catch
		{
			return StatusCode(500, "Internal server error");
		}
	}

}
