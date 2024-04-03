using FluentValidation;

namespace HW4.Validators;

public class BaseProductInfoValidator : AbstractValidator<ProductInfo>
{
	public BaseProductInfoValidator()
	{
		RuleFor(product => product.ProductNumber).NotEmpty().WithMessage("Product number must be specified.");
		RuleFor(product => product.ProductName).NotEmpty().WithMessage("Product name must be specified.");
		RuleFor(product => product.ProductType).IsInEnum().WithMessage("Invalid product type.");
		RuleFor(product => product.ProductWeight).GreaterThan(0).WithMessage("Product weight must be greater than 0.");
		RuleFor(product => product.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
		RuleFor(product => product.CreatedAt).NotEmpty().WithMessage("Creation date must be specified.");
		RuleFor(product => product.StockNumber).NotEmpty().WithMessage("Stock number must be specified.");
	}
}
