using Services.Identity.Domain.AggregateModels.Role;
using Services.Identity.Infrastructure.Models;

namespace Services.Identity.Infrastructure.Mappers
{
    public static class RoleMapper
    {
        public static Role Map(RoleDto role)
        {
            return new Role(role.Id, role.DisplayName);
        }
    }
}
