using System.ComponentModel.DataAnnotations;

namespace Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;

public record ProductDto
{
	public Guid ProductID { get; init; }
	public string? Name { get; init; }
	public string? Description { get; init; }
	public double? Price { get; init; }
	public bool? Availability { get; init; }
	public string? CategoryName { get; init; }
	public DateTime CreationDate { get; init; }
	public string? ImageUrl { get; init; }
    public Guid UserID { get; init; }
}

public record ProductForManipulationDto
{
	public string? Name { get; init; }
	public string? Description { get; init; }
	public double? Price { get; init; }
	public bool? Availability { get; init; }
	public string? CategoryName { get; init; }
	public DateTime CreationDate { get; init; }
	public string? ImageUrl { get; init; }
}

public record ProductForUpdateDto : ProductForManipulationDto;
public record ProductForCreationDto : ProductForManipulationDto;