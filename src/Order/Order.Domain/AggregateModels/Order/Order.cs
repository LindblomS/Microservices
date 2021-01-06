namespace Services.Order.Domain
{
    using Services.Order.Domain.SeedWork;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Order : Entity, IAggregateRoot
    {
        private List<OrderItem> _orderItems;
        private Guid _customerId;

        public Order(Guid id, Guid customerId)
        {
            Id = id;
            _customerId = customerId;
            _orderItems = new List<OrderItem>();
        }

        public Guid CustomerId
        {
            get => _customerId;
            private set { _customerId = value; }
        }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public void AddOrderItem(OrderItem orderItem)
        {
            var existingOrderItem = _orderItems.FirstOrDefault(x => x.Name == orderItem.Name);

            if (existingOrderItem != null)
            {
                existingOrderItem.AddQuantity(orderItem.Quantity);
            }
            else
            {
                _orderItems.Add(orderItem);
            }
        }
    }
}
