using FluentValidation;
using FluentValidation.Results;
using Inno_Shop.Services.ProductAPI.Core.Application.Commands;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Validators;

public sealed class UpdateProductCommandValidator :
	AbstractValidator<UpdateProductCommand>
{
	public UpdateProductCommandValidator()
	{
		RuleFor(c => c.Product.Name)
			.NotEmpty().WithMessage("Name is required.")
			.Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");

        RuleFor(c => c.Product.CategoryName)
            .NotEmpty().WithMessage("CategoryName is required.")
            .Length(1, 150).WithMessage("CategoryName must be between 1 and 150 characters.");

        RuleFor(c => c.Product.CreationDate)
           .NotEmpty().WithMessage("CreationDate is required.")
           .GreaterThan(new DateTime(1950, 1, 1)).WithMessage("CreationDate must be greater than 01.01.1950.");

        RuleFor(c => c.Product.Description)
			.NotEmpty().WithMessage("Description is required.")
			.Length(1, 500).WithMessage("Description must be between 1 and 500 characters.");

		RuleFor(c => c.Product.Price)
			.NotEmpty().WithMessage("Price is required.")
			.GreaterThan(0).WithMessage("Price must be greater than 0.");
	}

	public override ValidationResult Validate(ValidationContext<UpdateProductCommand> context)
	{
		return context.InstanceToValidate.Product is null 
			? new ValidationResult(new[] { 
				new ValidationFailure("ProductForUpdateDto", "ProductForUpdateDto object is null") 
			}) : base.Validate(context);
	}

}

