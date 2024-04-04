using FluentValidation;
using HW4.BusinessLogic.Services.DataTransferObjects;


namespace HW4.WebHost.Validators;

public class GetProductsByFilterRequestValidator : AbstractValidator<GetProductsByFilterRequest>
{
	public GetProductsByFilterRequestValidator()
	{
		RuleFor(request => request.Skip).GreaterThanOrEqualTo(0).WithMessage("The skip value must be greater than or equal to 0.");
		RuleFor(request => request.Take).GreaterThan(0).WithMessage("The take value must be greater than 0.");
		RuleFor(request => request.FilterBy).IsInEnum().WithMessage("Invalid filtering type.");
		RuleFor(request => request.StockNumber).GreaterThanOrEqualTo(0).WithMessage("The stock number must be greater than or equal to 0.");
		RuleFor(request => request.ProductType).IsInEnum().WithMessage("Invalid product type.");
		RuleFor(request => request.DateFrom).NotEmpty().WithMessage("Creation date must be specified.");
	}
}
