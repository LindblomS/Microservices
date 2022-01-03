namespace Basket.Domain.Exceptions;

public class CreateBasketException : BasketDomainException
{
    public CreateBasketException(string detail) : base($"Could not create basket. {detail}")
    {

    }
}
