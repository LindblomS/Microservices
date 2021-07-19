using Services.Identity.Domain.AggregateModels;
using Services.Identity.Infrastructure.Models;

namespace Services.Identity.Infrastructure.Mappers
{
    public static class ClaimMapper
    {
        public static Claim Map(ClaimDto claim)
        {
            return new Claim(claim.Type, claim.Value);
        }
    }
}
