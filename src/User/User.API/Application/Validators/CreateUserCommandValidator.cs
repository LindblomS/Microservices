namespace Services.User.API.Application.Validators
{
    using FluentValidation;
    using Services.User.API.Application.Models.Commands;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Username).NotEmpty();
            RuleFor(c => c.PasswordHash).NotEmpty();
            RuleFor(c => c.Claims).NotNull();
            RuleFor(c => c.Roles).NotNull();
        }
    }
}
