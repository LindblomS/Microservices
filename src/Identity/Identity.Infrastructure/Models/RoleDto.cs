namespace Services.Identity.Infrastructure.Models
{
    public record RoleDto
    {
        public string Id { get; init; }
        public string DisplayName { get; init; }
    }
}
