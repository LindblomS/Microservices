namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class SetPaidOrderStatusCommandValidator : AbstractValidator<SetPaidOrderStatus>
{
    public SetPaidOrderStatusCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}


