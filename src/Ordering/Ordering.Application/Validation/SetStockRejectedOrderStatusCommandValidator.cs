namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class SetStockRejectedOrderStatusCommandValidator : AbstractValidator<SetStockRejectedOrderStatusCommand>
{
    public SetStockRejectedOrderStatusCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
        RuleFor(c => c.StockItems).NotEmpty();
        RuleForEach(c => c.StockItems).NotEmpty();
    }
}


