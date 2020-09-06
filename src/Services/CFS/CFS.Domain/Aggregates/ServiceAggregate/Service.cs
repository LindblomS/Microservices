using CFS.Domain.SeedWork;
using System;

namespace CFS.Domain.Aggregates
{
    public class Service : Entity, IAggregateRoot
    {
        private int _facilityId;
        private DateTime _startDate;
        private DateTime? _stopDate;

        public int FacilityId => _facilityId;
        public DateTime StartDate => _startDate;
        public DateTime? StopDate => _stopDate;

        public Service(int serviceId, int facilityId, DateTime startDate, DateTime? stopDate)
        {
            Id = serviceId;
            _facilityId = facilityId;
            _startDate = startDate != default ? startDate : throw new ArgumentNullException(nameof(startDate));
            _stopDate = stopDate != default ? stopDate : null;
        }
    }
}
