namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class SetStockConfirmedOrderStatusCommandValidator : AbstractValidator<SetStockConfirmedOrderStatusCommand>
{
    public SetStockConfirmedOrderStatusCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}


