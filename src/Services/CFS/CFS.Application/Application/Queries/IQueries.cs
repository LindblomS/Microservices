using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFS.Application.Application.Queries
{
    public interface IQueries
    {
        Task<CustomerViewModel> GetCustomer(int customerId);
        Task<List<CustomerViewModel>> GetCustomers();
        Task<FacilityViewModel> GetFacility(int faciltyId);
        Task<List<FacilityViewModel>> GetFacilitiesOnCustomer(int customerId);
        Task<List<FacilityViewModel>> GetFacilities();
        Task<ServiceViewModel> GetService(int serviceId);
        Task<List<ServiceViewModel>> GetServicesOnCustomer(int customerId);
        Task<List<ServiceViewModel>> GetServices();
    }
}
