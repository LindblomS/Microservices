namespace Services.User.Domain.AggregateModels.User
{
    using Services.User.Domain.SeedWork;
    using System;
    using System.Threading.Tasks;

    public interface IUserRepository : IRepository<User>
    {
        void Create(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User> GetAsync(Guid id);
    }
}
