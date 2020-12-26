namespace Services.Customer.API.Application.Queries
{
    using System;

    public class CustomerViewModel
    {
        public CustomerViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
