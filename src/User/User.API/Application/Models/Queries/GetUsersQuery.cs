namespace Services.User.API.Application.Models.Queries
{
    using MediatR;
    using Services.User.API.Application.Models.Results;
    using System.Collections.Generic;

    public class GetUsersQuery : IRequest<QueryResult<IEnumerable<UserReadModel>>>
    {
    }
}
