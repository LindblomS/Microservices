﻿namespace Services.Order.API.Application.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderQueries
    {
        Task<IEnumerable<OrderViewModel>> GetOrdersAsync(Guid customerId);
    }

}
