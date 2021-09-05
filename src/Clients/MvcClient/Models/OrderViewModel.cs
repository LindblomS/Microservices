namespace MvcClient.Models
{
    using System;
    using System.Collections.Generic;

    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
    }
}
