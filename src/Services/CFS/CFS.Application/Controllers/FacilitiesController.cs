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
    [Route("api/v1/facilites")]
    [ApiController]
    public class FacilitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IQueries _queries;
        private readonly ILogger<FacilitiesController> _logger;

        public FacilitiesController(IMediator mediator, IQueries queries, ILogger<FacilitiesController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("id:int")]
        [ProducesResponseType(typeof(FacilityViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetFacilitiesAsync(int id)
        {
            try
            {
                var facility = await _queries.GetFacility(id);
                return Ok(facility);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FacilityViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FacilityViewModel>>> GetFacilitesAsync()
        {
            var facilities = await _queries.GetFacilities();
            return Ok(facilities);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateFacilityAsync([FromBody] UpdateFacilityCommand command)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName} - ({@Command})",
                command.GetGenericTypeName(),
                command);

            var commandResult = await _mediator.Send(command);

            if (commandResult)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateFacilityAsync([FromBody] CreateFacilityCommand command)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName} - ({@Command})",
                command.GetGenericTypeName(),
                command);

            var commandResult = await _mediator.Send(command);

            if (commandResult)
                return Ok();
            else
                return BadRequest();
        }
    }
}
