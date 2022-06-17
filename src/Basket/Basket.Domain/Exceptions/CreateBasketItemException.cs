namespace Basket.Domain.Exceptions;
public class CreateBasketItemException : BasketDomainException
{
    public CreateBasketItemException(string detail) : base($"Could not create basket item. {detail}")
    {

    }
}
