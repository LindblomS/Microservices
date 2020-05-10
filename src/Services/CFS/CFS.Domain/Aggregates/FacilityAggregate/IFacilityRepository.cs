using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.FacilityAggregate
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        Task Add(Facility facility);
        Task Update(Facility facility);
        Task<Facility> GetFacility(int facilityId);
    }
}
