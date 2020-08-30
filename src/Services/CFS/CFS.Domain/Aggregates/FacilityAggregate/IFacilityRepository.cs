using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates
{
    public interface IFacilityRepository
    {
        Task Add(Facility facility);
        Task Update(Facility facility);
        Task Delete(int facilityId);
        Task<Facility> GetFacility(int facilityId);
    }
}
