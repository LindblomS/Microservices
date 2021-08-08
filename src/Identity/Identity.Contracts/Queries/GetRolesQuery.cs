namespace Services.Identity.Contracts.Queries
{
    using MediatR;
    using Services.Identity.Contracts.Models;
    using Services.Identity.Contracts.Models.Results;
    using System.Collections.Generic;

    public class GetRolesQuery : IRequest<QueryResult<IEnumerable<RoleReadModel>>>
    {
    }
}
