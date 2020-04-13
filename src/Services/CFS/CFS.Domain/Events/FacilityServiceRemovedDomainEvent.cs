using MediatR;

namespace CFS.Domain.Events
{
    public class FacilityServiceRemovedDomainEvent : INotification
    {
        public int FacilityId { get; private set; }
        public int ServiceId { get; private set; }

        public FacilityServiceRemovedDomainEvent(int facilityId, int serviceId)
        {
            FacilityId = facilityId;
            ServiceId = serviceId;
        }
    }
}
