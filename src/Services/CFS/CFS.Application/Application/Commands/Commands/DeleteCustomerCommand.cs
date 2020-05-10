using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class DeleteCustomerCommand : IRequest<bool>
    {
        [DataMember]
        public int Id { get; private set; }

        public DeleteCustomerCommand(int id)
        {
            Id = id;
        }
    }
}
