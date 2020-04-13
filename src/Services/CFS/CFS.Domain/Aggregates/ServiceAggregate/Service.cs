using CFS.Domain.SeedWork;
using System;

namespace CFS.Domain.Aggregates.ServiceAggregate
{
    public class Service : Entity, IAggregateRoot
    {
        private DateTime _startDate;
        private DateTime? _stopDate;

        public DateTime StartDate => _startDate;
        public DateTime? StopDate => _stopDate;

        public Service(int id, DateTime startDate, DateTime? stopDate)
        {
            Id = id;
            _startDate = startDate != default ? startDate : throw new ArgumentNullException(nameof(startDate));
            _stopDate = stopDate != default ? stopDate : null;
        }
    }
}
