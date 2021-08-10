namespace Identity.API.Application.Validators
{
    using FluentValidation;
    using Services.Identity.Contracts.Models;

    public class ClaimValidator : AbstractValidator<Claim>
    {
        public ClaimValidator()
        {
            RuleFor(c => c.Type).NotEmpty();
            RuleFor(c => c.Value).NotEmpty();
        }
    }
}
