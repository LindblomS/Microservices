using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.UnitTests
{
    public class AddressBuilder
    {
        public Address Build()
        {
            return new Address("street", "city", "state", "country", "zipcode");
        }
    }

    public class OrderBuilder
    {
        private readonly Order order;

        public OrderBuilder(Address address)
        {
            order = new Order(
                "userId",
                "fakeName",
                address,
                cardTypeId: 5,
                cardNumber: "12",
                cardSecurityNumber: "123",
                cardHolderName: "name",
                cardExpiration: DateTime.UtcNow,
                null,
                null);
        }

        public OrderBuilder AddOne(
            int productId,
            string productName,
            decimal unitPrice,
            decimal discount,
            string pictureUrl,
            int units = 1)
        {
            order.AddOrderItem(productId, productName, pictureUrl, unitPrice, discount, units);
            return this;
        }

        public Order Build()
        {
            return order;
        }
    }
}
