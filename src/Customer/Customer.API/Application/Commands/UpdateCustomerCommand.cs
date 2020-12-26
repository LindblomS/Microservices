namespace Services.Customer.API.Application.Commands
{
    using MediatR;
    using System;
    using System.Runtime.Serialization;

    public class UpdateCustomerCommand : IRequest<bool>
    {
        public UpdateCustomerCommand(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        [DataMember]
        public string Name { get; private set; }
        public Guid Id { get; private set; }
    }
}
