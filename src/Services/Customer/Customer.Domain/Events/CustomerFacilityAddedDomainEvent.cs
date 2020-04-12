using MediatR;

namespace CFS.Domain.Events
{
    public class CustomerFacilityAddedDomainEvent : INotification
    {
        public int CustomerId { get; private set; }
        public int FacilityId { get; private set; }

        public CustomerFacilityAddedDomainEvent(int customerId, int facilityId)
        {
            CustomerId = CustomerId;
            FacilityId = facilityId;
        }
    }
}
