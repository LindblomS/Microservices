namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand>
{
    public IdentifiedCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}


