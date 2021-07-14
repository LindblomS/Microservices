﻿namespace Services.Identity.Domain.AggregateModels.User
{
    using Services.Identity.Domain.Domain.SeedWork;
    using System;
    using System.Threading.Tasks;

    public interface IUserRepository : IRepository<User>
    {
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task GetAsync(Guid id);
    }
}
