using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.FacilityAggregate
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        void Add(Facility facility);
        void Update(Facility facility);
        Facility GetFacility(int facilityId);
    }
}
