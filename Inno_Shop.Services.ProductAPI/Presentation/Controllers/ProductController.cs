﻿using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
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
		var products = await _service.GetProductsAsync(trackChanges: false);
		return Ok(products);
	}
}
