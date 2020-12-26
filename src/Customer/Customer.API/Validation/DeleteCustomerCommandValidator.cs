namespace Services.Customer.API.Validation
{
    using FluentValidation;
    using Services.Customer.API.Application.Commands;

    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
