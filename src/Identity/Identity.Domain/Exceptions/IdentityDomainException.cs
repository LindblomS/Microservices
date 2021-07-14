namespace Services.Identity.Domain.Exceptions
{
    using System;

    public class IdentityDomainException : Exception
    {
        public IdentityDomainException(string message) : base(message)
        {
        }
    }
}
