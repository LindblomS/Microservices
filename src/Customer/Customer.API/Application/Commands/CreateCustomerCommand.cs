namespace Services.Customer.API.Application.Commands
{
    using MediatR;
    using System;
    using System.Runtime.Serialization;

    public class CreateCustomerCommand : IRequest<Guid>
    {
        public CreateCustomerCommand(string name)
        {
            Name = name;
        }

        [DataMember]
        public string Name { get; private set; }
    }
}
