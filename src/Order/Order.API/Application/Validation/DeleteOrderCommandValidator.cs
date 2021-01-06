namespace Services.Order.API.Application.Validation
{
    using FluentValidation;
    using Services.Order.API.Application.Commands.Commands;

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(order => order.OrderId).NotEmpty();
        }
    }
}
