namespace Identity.API.Application.Validators
{
    using FluentValidation;
    using Services.Identity.Contracts.Commands;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleForEach(c => c.Claims).SetValidator(new ClaimValidator());
            RuleForEach(c => c.Roles).NotEmpty();
        }
    }
}
