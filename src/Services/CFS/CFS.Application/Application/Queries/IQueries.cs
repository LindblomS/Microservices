using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFS.Application.Application.Queries
{
    public interface IQueries
    {
        Task<CustomerViewModel> GetCustomer(int id);
        Task<IList<CustomerViewModel>> GetCustomers();
        Task<FacilityViewModel> GetFacility(int id);
        Task<IList<FacilityViewModel>> GetFacilitiesOnCustomer(int id);
        Task<IList<FacilityViewModel>> GetFacilities();
        Task<ServiceViewModel> GetService(int id);
        Task<IList<ServiceViewModel>> GetServicesOnCustomer(int id);
        Task<IList<ServiceViewModel>> GetServices();
    }
}
