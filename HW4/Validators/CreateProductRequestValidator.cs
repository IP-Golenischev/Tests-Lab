using FluentValidation;
using HW4.DataTransferObject;

namespace HW4.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
	public CreateProductRequestValidator()
	{
		RuleFor(product => product.ProductName)
			.NotEmpty().WithMessage("Product name must not be empty.");

		RuleFor(product => product.ProductType)
			.IsInEnum().WithMessage("Invalid product type.");

		RuleFor(product => product.ProductWeight)
			.GreaterThan(0).WithMessage("Product weight must be greater than 0.");

		RuleFor(product => product.Price)
			.GreaterThan(0).WithMessage("Product price must be greater than 0.");

		RuleFor(product => product.StockNumber)
			.GreaterThan(0).WithMessage("Stock number must be greater than 0.");
	}
}
