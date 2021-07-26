using System;

namespace Services.Identity.Infrastructure.Models
{
    public record UserDto (Guid id, string username, string passwordHash);
}
