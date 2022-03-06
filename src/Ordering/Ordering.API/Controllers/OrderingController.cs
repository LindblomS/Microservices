namespace Ordering.API.Controllers;

using global::MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Contracts.Requests;

[ApiController]
[Route("api/[controller]")]
public class OrderingController : ControllerBase
{
    readonly IMediator mediator;

    public OrderingController(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(GetOrder.Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetOrder.Order>> Get(string id)
    {
        if (!Guid.TryParse(id, out Guid guid))
            return BadRequest("Invalid id. Id must be a valid GUID");

        return Ok(await mediator.Send(new GetOrder(guid)));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetOrders.Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetOrders.Order>>> Get()
    {
        var result = await mediator.Send(new GetOrders());
        return Ok(result);
    }
}
