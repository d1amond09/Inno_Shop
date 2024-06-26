using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.ProductAPI.Models.DataTransferObjects;

public record ProductDto
{
	public int ProductId { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }

	[Range(1, double.MaxValue)]
	public double? Price { get; set; }
	public string? CategoryName { get; set; }
	public DateTime CreationDate { get; set; }
	public string? ImageUrl { get; set; }
}

public record ProductForUpdateDto : ProductDto;
public record ProductForCreationDto : ProductDto;