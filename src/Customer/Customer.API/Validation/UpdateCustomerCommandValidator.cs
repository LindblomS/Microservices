namespace Services.Customer.API.Validation
{
    using FluentValidation;
    using Services.Customer.API.Application.Commands;

    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
