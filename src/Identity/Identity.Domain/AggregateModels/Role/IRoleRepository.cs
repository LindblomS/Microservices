namespace Services.Identity.Domain.AggregateModels.Role
{
    using Services.Identity.Domain.Domain.SeedWork;
    using System.Threading.Tasks;

    public interface IRoleRepository : IRepository<Role>
    {
        void Create(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<Role> GetAsync(string id);
    }
}
