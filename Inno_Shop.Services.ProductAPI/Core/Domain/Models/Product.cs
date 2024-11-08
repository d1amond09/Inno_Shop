using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.ProductAPI.Domain.Models;

public class Product
{
	[Key]
	public Guid ProductID { get; set; }
	[Required]
	public string? Name { get; set; }
	public string? Description { get; set; }
	[Required]
	[Range(1, double.MaxValue)]
	public double? Price { get; set; }
	public string? CategoryName { get; set; }
	public bool? Availability { get; set; }
	public DateTime CreationDate { get; set; }
	public string? ImageUrl { get; set; }
	public Guid UserID { get; set; }

}
