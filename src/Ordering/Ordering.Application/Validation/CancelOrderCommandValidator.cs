namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}
