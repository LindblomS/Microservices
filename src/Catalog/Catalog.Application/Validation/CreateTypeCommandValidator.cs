namespace Catalog.Application.Validation;

using Catalog.Contracts.Commands;
using FluentValidation;

public class CreateTypeCommandValidator : AbstractValidator<CreateTypeCommand>
{
    public CreateTypeCommandValidator()
    {
        RuleFor(c => c.Type)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Type cannot be empty and must not exceed 250 characters")
            .Length(250)
            .WithMessage(c => $"Type cannot not exceed 250 characters, Type was {c.Type.Length} characters");

    }
}
