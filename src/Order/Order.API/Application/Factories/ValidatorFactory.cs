namespace Services.Order.API.Application.Factories
{
    using FluentValidation;
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _sevices;

        public ValidatorFactory(IServiceProvider sevices)
        {
            _sevices = sevices ?? throw new ArgumentNullException(nameof(sevices));
        }

        public IValidator<T> GetValidator<T>()
        {
            return _sevices.GetService<IValidator<T>>();
        }

        public IValidator GetValidator(Type type)
        {
            return (IValidator)_sevices.GetService(type);
        }
    }
}
