namespace Services.Customer.API.Application.Commands
{
    using MediatR;
    using System;

    public class DeleteCustomerCommand : IRequest<bool>
    {
        public DeleteCustomerCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
