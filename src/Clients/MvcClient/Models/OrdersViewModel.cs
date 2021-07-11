namespace MvcClient.Models
{
    using System.Collections.Generic;

    public class OrdersViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
