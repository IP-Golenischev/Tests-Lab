using FluentValidation;
using HW4.BusinessLogic.Services.DataTransferObjects;

namespace HW4.Validators
{
	public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
	{
		public UpdateProductRequestValidator()
		{
			RuleFor(x => x.ProductNumber).NotEmpty().WithMessage("Product number must be specified.");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
		}
	}
}
