namespace Services.User.API.Application.Validators
{
    using FluentValidation;
    using Services.User.API.Application.Models;

    public class ClaimValidator : AbstractValidator<Claim>
    {
        public ClaimValidator()
        {
            RuleFor(c => c.Type).NotEmpty();
            RuleFor(c => c.Value).NotEmpty();
        }
    }
}
