using FluentValidation;
using ProductService.Dtos;

namespace ProductService.Validators;
public class ProductRequestDtoValidator : AbstractValidator<ProductRequestDto>
{
    public ProductRequestDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}
