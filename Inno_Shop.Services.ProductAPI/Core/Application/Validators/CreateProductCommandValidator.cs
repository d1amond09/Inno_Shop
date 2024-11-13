using FluentValidation;
using FluentValidation.Results;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Validators;

public sealed class CreateProductCommandValidator :
	AbstractValidator<CreateProductCommand>
{
	public CreateProductCommandValidator()
	{
		RuleFor(c => c.Product.Name)
			.NotEmpty().WithMessage("Name is required.")
			.Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");

        RuleFor(c => c.Product.CategoryName)
            .NotEmpty().WithMessage("CategoryName is required.")
            .Length(1, 150).WithMessage("CategoryName must be between 1 and 150 characters.");

		RuleFor(c => c.Product.Availability)
			.NotEmpty().WithMessage("Availability is required.");

		RuleFor(c => c.Product.Price)
			.NotEmpty().WithMessage("Price is required.")
			.GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(c => c.Product.Description)
            .Length(1, 500).WithMessage("Description must be between 1 and 500 characters.");
    }

	public override ValidationResult Validate(ValidationContext<CreateProductCommand> context)
	{
		return context.InstanceToValidate.Product is null 
			? new ValidationResult(new[] { 
				new ValidationFailure("ProductForCreationDto", "ProductForCreationDto object is null") 
			}) : base.Validate(context);
	}

}

