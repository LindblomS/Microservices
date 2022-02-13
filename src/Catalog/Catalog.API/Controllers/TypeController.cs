namespace Catalog.API.Controllers;

using Catalog.Contracts.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class TypeController : ControllerBase
{
    readonly IMediator mediator;

    public TypeController(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTypeCommand command)
    {
        _ = await mediator.Send(command);
        return Ok();
    }
}
