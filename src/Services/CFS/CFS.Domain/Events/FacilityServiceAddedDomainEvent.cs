using MediatR;

namespace CFS.Domain.Events
{
    public class FacilityServiceAddedDomainEvent : INotification
    {
        public int FacilityId { get; private set; }
        public int ServiceId { get; private set; }

        public FacilityServiceAddedDomainEvent(int facilityId, int serviceId)
        {
            FacilityId = facilityId;
            ServiceId = serviceId;
        }
    }
}
