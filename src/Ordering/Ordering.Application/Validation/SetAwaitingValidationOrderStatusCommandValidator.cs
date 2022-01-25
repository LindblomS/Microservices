namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class SetAwaitingValidationOrderStatusCommandValidator : AbstractValidator<SetAwaitingValidationOrderStatusCommand>
{
    public SetAwaitingValidationOrderStatusCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}


