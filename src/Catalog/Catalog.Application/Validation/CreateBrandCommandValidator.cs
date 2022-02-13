namespace Catalog.Application.Validation;

using Catalog.Contracts.Commands;
using FluentValidation;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(c => c.Brand)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Brand cannot be empty and must not exceed 250 characters")
            .MaximumLength(250)
            .WithMessage(c => $"Brand cannot not exceed 250 characters. Brand was {c.Brand.Length} characters");
    }

}
