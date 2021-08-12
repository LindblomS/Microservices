using Services.User.Domain.AggregateModels.Role;
using Services.User.Infrastructure.Models;

namespace Services.User.Infrastructure.Mappers
{
    public static class RoleMapper
    {
        public static Role Map(RoleDto role)
        {
            return new Role(role.id, role.displayName);
        }
    }
}
