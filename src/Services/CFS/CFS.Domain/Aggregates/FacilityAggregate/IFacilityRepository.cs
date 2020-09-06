using CFS.Domain.SeedWork;
using System.Threading.Tasks;

namespace CFS.Domain.Aggregates
{
    public interface IFacilityRepository
    {
        Task<int> Add(Facility facility);
        Task<int> Update(Facility facility);
        Task<int> Delete(Facility facility);
        Task<Facility> GetFacility(int facilityId);
    }
}
