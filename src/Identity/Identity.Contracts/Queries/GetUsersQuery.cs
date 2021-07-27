namespace Services.Identity.Contracts.Queries
{
    using MediatR;
    using Services.Identity.Contracts.Models;
    using Services.Identity.Contracts.Results;
    using System.Collections.Generic;

    public class GetUsersQuery : IRequest<QueryResult<IEnumerable<User>>>
    {
    }
}
