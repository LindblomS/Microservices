namespace Services.Customer.API.Application.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerQueries
    {
        Task<IList<CustomerViewModel>> GetAsync();
        Task<CustomerViewModel> GetAsync(Guid customerId);
    }
}
