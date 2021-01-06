namespace Services.Order.Domain
{
    using Services.Order.Domain.SeedWork;
    using System;

    public class OrderItem : Entity
    {
        private Guid _orderId;
        private string _name;
        private int _quantity;

        public OrderItem(Guid orderId, string name, int quantity)
        {
            _orderId = orderId;
            _name = name;
            _quantity = quantity;
        }

        public Guid OrderId => _orderId;
        public string Name => _name;
        public int Quantity => _quantity;

        public void AddQuantity(int value)
        {
            _quantity += value;
        }
    }
}
