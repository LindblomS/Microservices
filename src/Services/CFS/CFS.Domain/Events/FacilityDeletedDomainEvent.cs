using MediatR;

namespace CFS.Domain.Events
{
    public class FacilityDeletedDomainEvent : INotification
    {
        public int FacilityId { get; private set; }

        public FacilityDeletedDomainEvent(int facilityId)
        {
            FacilityId = facilityId;
        }
    }
}
