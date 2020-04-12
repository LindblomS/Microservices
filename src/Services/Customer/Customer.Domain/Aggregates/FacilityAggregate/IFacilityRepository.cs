using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates.FacilityAggregate
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        Facility Add(Facility facility);
        void Update(Facility facility);
        Task<Facility> GetCustomerAsync(int facilityId);
    }
}
