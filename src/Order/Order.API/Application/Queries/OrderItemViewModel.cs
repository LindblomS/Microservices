namespace Services.Order.API.Application.Queries
{
    public class OrderItemViewModel
    {
        public OrderItemViewModel(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
