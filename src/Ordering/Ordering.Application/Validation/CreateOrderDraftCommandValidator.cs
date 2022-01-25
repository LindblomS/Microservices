namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class CreateOrderDraftCommandValidator : AbstractValidator<CreateOrderDraftCommand>
{
    public CreateOrderDraftCommandValidator()
    {
        RuleFor(c => c.BuyerId).NotEmpty();

        var orderItemValidator = new OrderItemValidator();
        RuleForEach(c => c.OrderItems)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(orderItemValidator);
    }

    class OrderItemValidator : AbstractValidator<CreateOrderDraftCommand.OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(i => i.ProductId).NotEmpty();
            RuleFor(i => i.ProductName).NotEmpty();
            RuleFor(i => i.UnitPrice).GreaterThan(0);
            RuleFor(i => i.Units).GreaterThan(0);
        }
    }
}


