using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class DeleteCustomerCommand : IRequest<bool>
    {
        [DataMember]
        public int CustomerId { get; set; }

        public DeleteCustomerCommand()
        {

        }

        public DeleteCustomerCommand(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
