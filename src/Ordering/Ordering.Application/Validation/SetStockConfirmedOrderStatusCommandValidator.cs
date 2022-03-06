namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class SetStockConfirmedOrderStatusCommandValidator : AbstractValidator<SetStockConfirmedOrderStatus>
{
    public SetStockConfirmedOrderStatusCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}


