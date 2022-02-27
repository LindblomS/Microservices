namespace Ordering.Application.Validation;

using FluentValidation;
using Ordering.Application.Services;
using Ordering.Contracts.Requests;

public class GetOrderValidator : AbstractValidator<GetOrder>
{
    public GetOrderValidator(IValidationService service)
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(x => service.OrderExists(x))
            .WithMessage(x => $"Order with id {x.Id} does not exists");
    }
}


