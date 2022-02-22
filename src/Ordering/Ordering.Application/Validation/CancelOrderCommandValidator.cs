namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Commands;
using Ordering.Application.Services;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator(IValidationService service)
    {
        RuleFor(c => c.OrderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(c => service.OrderExists(c))
            .WithMessage(c => $"Order with id {c.OrderId} does not exists");
    }
}
