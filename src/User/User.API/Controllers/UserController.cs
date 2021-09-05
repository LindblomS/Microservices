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
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Authorize(Policy = "user")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand request, [FromHeader(Name = "request_id")] string requestId)
        {
            if (Guid.TryParse(requestId, out Guid guid) && guid != default)
            {
                var command = new IdentifiedCommand<CreateUserCommand, CommandResult>(request, guid);

                _logger.LogInformation(
                    "----- Sending command: {CommandName}: {CommandId} ({@Command})",
                    typeof(CreateUserCommand).Name,
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
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand request, [FromHeader(Name = "request_id")] string requestId)
        {
            if (Guid.TryParse(requestId, out Guid guid) && guid != default)
            {
                var command = new IdentifiedCommand<UpdateUserCommand, CommandResult>(request, guid);

                _logger.LogInformation(
                    "----- Sending command: {CommandName}: {CommandId} ({@Command})",
                    typeof(UpdateUserCommand).Name,
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
        [ProducesResponseType(typeof(IEnumerable<UserReadModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserReadModel>>> GetUsers()
        {
            var result = await _mediator.Send(new GetUsersQuery());

            if (result.IsException())
            {
                _logger.LogError(result.Exception, "error");
                return Problem("internal server error", null, (int)HttpStatusCode.InternalServerError);
            }

            return Ok(result.Result);
        }
    }
}
