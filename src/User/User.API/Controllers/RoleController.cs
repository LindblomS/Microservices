namespace Services.User.API.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.User.API.Application.Models;
    using Services.User.API.Application.Models.Commands;
    using Services.User.API.Application.Models.Queries;
    using Services.User.API.Application.Models.Results;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    [Route("api/{controller}")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IMediator mediator, ILogger<RoleController> logger)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Authorize(Policy = "user")]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand request, [FromHeader(Name = "request_id")] string requestId)
        {
            if (Guid.TryParse(requestId, out Guid guid) && guid != default)
            {
                var command = new IdentifiedCommand<CreateRoleCommand, CommandResult>(request, guid);

                _logger.LogInformation(
                    "----- Sending command: {CommandName}: {CommandId} ({@Command})",
                    typeof(CreateRoleCommand).Name,
                    nameof(command.Id),
                    command.Id,
                    command);

                var result = await _mediator.Send(command);

                if (result is null)
                    return BadRequest();

                if (result.IsFailure())
                    return BadRequest(result.FailureMessage);

                if (result.IsException())
                {
                    _logger.LogError(result.Exception, "error");
                    return Problem("internal server error", null, (int)HttpStatusCode.InternalServerError);
                }

                return Ok();
            }

            return BadRequest("invalid request_id");
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Authorize(Policy = "user")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand request, [FromHeader(Name = "request_id")] string requestId)
        {
            if (Guid.TryParse(requestId, out Guid guid) && guid != default)
            {
                var command = new IdentifiedCommand<UpdateRoleCommand, CommandResult>(request, guid);

                _logger.LogInformation(
                    "----- Sending command: {CommandName}: {CommandId} ({@Command})",
                    typeof(UpdateRoleCommand).Name,
                    nameof(command.Id),
                    command.Id,
                    command);

                var result = await _mediator.Send(command);

                if (result is null)
                    return BadRequest();

                if (result.IsFailure())
                    return BadRequest(result.FailureMessage);

                if (result.IsException())
                {
                    _logger.LogError(result.Exception, "error");
                    return Problem("internal server error", null, (int)HttpStatusCode.InternalServerError);
                }

                return Ok();
            }

            return BadRequest("invalid request_id");
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoleWithoutClaimsReadModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _mediator.Send(new GetRolesQuery());

            if (result.IsException())
            {
                _logger.LogError(result.Exception, "error");
                return Problem("internal server error", null, (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Result);
        }
    }
}
