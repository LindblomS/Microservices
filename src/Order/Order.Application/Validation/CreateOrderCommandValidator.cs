namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.Username).NotEmpty();
        RuleFor(c => c.Address).NotNull();
        RuleFor(c => c.Card).NotNull();
        RuleFor(c => c.OrderItems).NotEmpty();

        var orderItemValidator = new OrderItemValidator();
        RuleForEach(c => c.OrderItems)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(orderItemValidator);
    }

    class OrderItemValidator : AbstractValidator<CreateOrderCommand.OrderItem>
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


