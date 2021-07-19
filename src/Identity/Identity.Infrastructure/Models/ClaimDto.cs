namespace Services.Identity.Infrastructure.Models
{
    public record ClaimDto
    {
        public string Type { get; init; }
        public string Value { get; init;  }
    }
}
