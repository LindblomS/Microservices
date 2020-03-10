using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private int? _buyerId;
        private int _orderStatusId;
        private int? _paymentMethodId;
        private readonly List<OrderItem> _orderItems;
        private DateTime _orderDate;
        private string _description;
        private bool _isDraft;

        public Address Address { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public int? GetBuyerId() => _buyerId;

        public static Order NewDraft()
        {
            var order = new Order();
            order._isDraft = true;
            return order;
        }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public Order(
            string userId,
            string userName,
            Address address,
            int cardTypeId,
            string cardNumber,
            string cardSecurityNumber,
            string cardHolderName,
            DateTime cardExpiration,
            int? buyerId,
            int? paymentMethod)
            : this()
        {
            _buyerId = buyerId;
            _paymentMethodId = paymentMethod;
            _orderStatusId = OrderStatus.Submitted.Id;
            _orderDate = DateTime.Now;
            Address = address;

            AddOrderStartedDomainEvent(
                userId, 
                userName, 
                cardTypeId, 
                cardNumber, 
                cardSecurityNumber, 
                cardHolderName, 
                cardExpiration);
        }

        public void AddOrderItem(
            int productId, 
            string productName, 
            string pictureUrl,
            decimal unitPrice, 
            decimal discount, 
            int units)
        {
            var existingOrderForProdukt = _orderItems.Where(o => o.ProductId == productId).SingleOrDefault();

            if (existingOrderForProdukt != null)
            {
                if (discount > existingOrderForProdukt.GetCurrentDiscount())
                    existingOrderForProdukt.SetNewDiscount(discount);

                existingOrderForProdukt.AddUnits(units);
            }
            else
            {
                var orderItem = new OrderItem(
                    productId,
                    productName,
                    pictureUrl,
                    unitPrice,
                    discount,
                    units);

                _orderItems.Add(orderItem);
            }
        }

        public void SetPaymentId(int id)
        {
            _paymentMethodId = id;
        }

        public void SetBuyerId(int id)
        {
            _buyerId = id;
        }

        public void SetAwaitingValidationStatus()
        {
            if (_orderStatusId == OrderStatus.Submitted.Id)
            {
                AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, OrderItems));
                _orderStatusId = OrderStatus.AwaitingValidation.Id;
            }
        }

        public void SetStockConfirmedStatus()
        {
            if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
            {
                AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));

                _orderStatusId = OrderStatus.StockConfirmed.Id;
                _description = "All the items were confirmed with available stock.";
            }
        }

        public void SetPaidStatus()
        {
            if (_orderStatusId == OrderStatus.StockConfirmed.Id)
            {
                AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));

                _orderStatusId = OrderStatus.Paid.Id;
                _description = "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"";
            }
        }

        public void SetShippedStatus()
        {
            if (_orderStatusId != OrderStatus.Paid.Id)
            {
                StatusChangeException(OrderStatus.Shipped);
            }

            _orderStatusId = OrderStatus.Shipped.Id;
            _description = "The order was shipped.";
            AddDomainEvent(new OrderShippedDomainEvent(this));
        }

        public void SetCancelledStatus()
        {
            if (_orderStatusId == OrderStatus.Paid.Id ||
                _orderStatusId == OrderStatus.Shipped.Id)
            {
                StatusChangeException(OrderStatus.Cancelled);
            }

            _orderStatusId = OrderStatus.Cancelled.Id;
            _description = $"The order was cancelled.";
            AddDomainEvent(new OrderCancelledDomainEvent(this));
        }

        public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
        {
            if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
            {
                _orderStatusId = OrderStatus.Cancelled.Id;

                var itemsStockRejectedProductNames = OrderItems
                    .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                    .Select(c => c.GetProductName());

                var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
                _description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
            }
        }

        private void StatusChangeException(OrderStatus orderStatusToChange)
        {
            throw new OrderingDomainException($"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}.");
        }

        public decimal GetTotal()
        {
            return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());
        }

        private void AddOrderStartedDomainEvent(
            string userId, 
            string userName, 
            int cardTypeId, 
            string cardNumber,
            string cardSecurityNumber,
            string cardHolderName,
            DateTime cardExperation)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(
                this, 
                userId, 
                userName, 
                cardTypeId, 
                cardNumber, 
                cardSecurityNumber, 
                cardHolderName, 
                cardExperation);

            this.AddDomainEvent(orderStartedDomainEvent);
        }

    }
}
