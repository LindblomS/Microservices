namespace Services.User.API.Application.Validators
{
    using FluentValidation;
    using Services.User.API.Application.Models.Commands;

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
