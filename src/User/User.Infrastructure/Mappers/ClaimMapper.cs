using Services.User.Domain.ValueObjects;
using Services.User.Infrastructure.Models;

namespace Services.User.Infrastructure.Mappers
{
    public static class ClaimMapper
    {
        public static Claim Map(ClaimDto claim)
        {
            return new Claim(claim.type, claim.value);
        }
    }
}
