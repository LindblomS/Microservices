namespace Services.Customer.API.Validation
{
    using FluentValidation;
    using Services.Customer.API.Application.Commands;

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(customer => customer.Name).NotEmpty();
        }
    }
}
