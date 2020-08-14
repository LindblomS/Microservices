using CFS.Application.Application.Commands.Commands;
using CFS.Application.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EventBus.Extensions;

namespace CFS.Application.Controllers
{
    [Route("api/v1/services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IQueries _queries;
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(IMediator mediator, IQueries queries, ILogger<ServicesController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("id:int")]
        [ProducesResponseType(typeof(ServiceViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetServiceAsync(int id)
        {
            try
            {
                var service = await _queries.GetService(id);
                return Ok(service);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ServiceViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ServiceViewModel>>> GetServicesAsync()
        {
            var services = await _queries.GetServices();
            return Ok(services);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateServiceAsync([FromBody] CreateServiceCommand command)
        {
            var commandResult = false;

            _logger.LogInformation(
                "----- Sending command: {CommandName} - ({@Command})",
                command.GetGenericTypeName(),
                command);

            commandResult = await _mediator.Send(command);

            if (commandResult)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateServiceAsync([FromBody] UpdateServiceCommand command)
        {
            var commandResult = false;

            _logger.LogInformation(
                "----- Sending command: {CommandName} - ({@Command})",
                command.GetGenericTypeName(),
                command);

            commandResult = await _mediator.Send(command);

            if (commandResult)
                return Ok();
            else
                return BadRequest();
        }

    }
}
