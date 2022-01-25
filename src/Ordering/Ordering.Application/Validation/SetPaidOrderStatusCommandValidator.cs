namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class SetPaidOrderStatusCommandValidator : AbstractValidator<SetPaidOrderStatusCommand>
{
    public SetPaidOrderStatusCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}


