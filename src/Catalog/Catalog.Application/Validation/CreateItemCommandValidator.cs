namespace Catalog.Application.Validation;

using Catalog.Application.Services;
using Catalog.Contracts.Commands;
using FluentValidation;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator(IValidationService service)
    {
        RuleFor(c => c.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(c => c.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(c => c.Brand)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Brand is missing")
            .Must(brand => service.BrandExists(brand))
            .WithMessage(c => $"Brand does not exists. Brand was {c.Brand}");

        RuleFor(c => c.Type)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Type is missing")
            .Must(type => service.TypeExists(type))
            .WithMessage(c => $"Type does not exists. Type was {c.Type}");
    }
}
