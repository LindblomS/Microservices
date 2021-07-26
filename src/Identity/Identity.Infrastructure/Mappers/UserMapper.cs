﻿using Services.Identity.Domain.AggregateModels;
using Services.Identity.Domain.AggregateModels.Role;
using Services.Identity.Domain.AggregateModels.User;
using Services.Identity.Infrastructure.Models;
using System.Collections.Generic;

namespace Services.Identity.Infrastructure.Mappers
{
    public static class UserMapper
    {
        public static User Map(UserDto user)
        {
            return new User(user.id, user.username, user.passwordHash, new List<Claim>(), new List<Role>());
        }
    }
}
