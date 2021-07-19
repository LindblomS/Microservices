using System;

namespace Services.Identity.Infrastructure.Models
{
    public record UserDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string PasswordHash { get; init; }
    }
}
