namespace Catalog.API.Controllers;

using Catalog.Contracts.Commands;
using Catalog.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")] 
[ApiController]
public class CatalogController : ControllerBase
{
    readonly IMediator mediator;

    public CatalogController(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        return Ok(await mediator.Send(new GetItemsQuery()));
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Item>> GetItem(string id)
    {
        if (!Guid.TryParse(id, out Guid guid))
            return BadRequest("Invalid id. Id must be a valid GUID");

        var item = await mediator.Send(new GetItemQuery(guid));

        if (item is null)
            return BadRequest($"Item with id {guid} was not found");

        return Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateItemCommand command)
    {
        _ = await mediator.Send(command);
        return Ok();
    }

    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        if (!Guid.TryParse(id, out Guid guid))
            return BadRequest("Invalid id. Id must be a valid GUID");

        _ = await mediator.Send(new DeleteItemCommand(guid));
        return Ok();
    }

    [Route("{id}")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateItemCommand command)
    {
        if (!Guid.TryParse(id, out Guid guid))
            return BadRequest("Invalid id. Id must be a valid GUID");

        _ = await mediator.Send(new InternalUpdateItemCommand(guid, command));
        return Ok();
    }
}
