﻿namespace Catalog.API.Controllers;

using Catalog.Contracts.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    readonly IMediator mediator;

    public BrandController(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBrandCommand command)
    {
        _ = await mediator.Send(command);
        return Ok();
    }
}