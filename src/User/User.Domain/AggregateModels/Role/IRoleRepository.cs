namespace Services.User.Domain.AggregateModels.Role
{
    using Services.User.Domain.SeedWork;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRoleRepository : IRepository<Role>
    {
        void Create(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<Role> GetAsync(string id);
        Task<IEnumerable<Role>> GetAsync(IEnumerable<string> roles);
    }
}
