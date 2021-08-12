using System;

namespace Services.User.Infrastructure.Models
{
    public record UserDto (Guid id, string username, string passwordHash);
}
