﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.ProductAPI.Models;

public class Product
{
	[Key]
	public int ProductId { get; set; }

	[Required]
	public string? Name { get; set; }

	public string? Description { get; set; }

	[Required]
	[Range(1, double.MaxValue)]
	public double? Price { get; set; }

    public string? CategoryName { get; set; }
    public DateTime CreationDate { get; set; }
    public string? ImageUrl { get; set; }

}