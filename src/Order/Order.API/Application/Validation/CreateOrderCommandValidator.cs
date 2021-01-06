namespace Services.Order.API.Application.Validation
{
    using FluentValidation;
    using Services.Order.API.Application.Commands.Commands;

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(order => order.CustomerId).NotEmpty();
            RuleFor(order => order.OrderItems).NotEmpty();
        }
    }
}
