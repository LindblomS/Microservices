namespace Services.User.Infrastructure.Mappers
{
    using Services.User.Domain.AggregateModels.Role;
    using Services.User.Domain.AggregateModels.User;
    using Services.User.Domain.ValueObjects;
    using Services.User.Infrastructure.Models;
    using System.Collections.Generic;

    public static class UserMapper
    {
        public static User Map(UserDto user)
        {
            return new User(user.id, user.username, user.passwordHash, new List<Claim>(), new List<Role>());
        }
    }
}
