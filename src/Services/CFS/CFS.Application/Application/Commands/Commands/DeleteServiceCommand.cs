using MediatR;
using System.Runtime.Serialization;

namespace CFS.Application.Application.Commands.Commands
{
    [DataContract]
    public class DeleteServiceCommand : IRequest<bool>
    {
        public int ServiceId { get; private set; }
        public DeleteServiceCommand(int serviceId)
        {
            ServiceId = serviceId;
        }
    }
}
