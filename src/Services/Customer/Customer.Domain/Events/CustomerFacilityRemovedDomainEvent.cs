using MediatR;

namespace CFS.Domain.Events
{
    public class CustomerFacilityRemovedDomainEvent : INotification
    {
        public int CustomerId { get; private set; }
        public int FacilityId { get; private set; }

        public CustomerFacilityRemovedDomainEvent(int customerId, int facilityId)
        {
            CustomerId = CustomerId;
            FacilityId = facilityId;
        }
    }
}
