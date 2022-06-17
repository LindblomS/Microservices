namespace Basket.Domain.AggregateModels
{
    using global::Basket.Domain.Exceptions;
    using System;

    public class BasketItem
    {
        public BasketItem(Guid productId)
        {
            if (productId == default)
                throw new CreateBasketItemException($"ProductId was not valid. ProductId was {productId}");

            ProductId = productId;
        }

        public Guid ProductId { get; private set; }
        public int Units { get; private set; }

        public void AddUnit()
        {
            Units++;
        }
    }
}
