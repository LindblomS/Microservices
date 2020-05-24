using CFS.Application.Application.Commands.Commands;
using CFS.Application.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EventBus.Extensions;

namespace CFS.Application.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CFSController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IQueries _queries;
        private readonly ILogger<CFSController> _logger;

        public CFSController(IMediator mediator, IQueries queries, ILogger<CFSController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("customers")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateCustomerAsync([FromBody] CreateCustomerCommand command)
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

        [Route("facilites")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateFacilityAsync([FromBody] CreateFacilityCommand command)
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

        [Route("services")]
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

        [Route("customers")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateCustomerAsync([FromBody] UpdateCustomerCommand command)
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

        [Route("facilites")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateFacilityAsync([FromBody] UpdateFacilityCommand command)
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

        [Route("services")]
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

        [Route("customers/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(CustomerViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _queries.GetCustomer(customerId);
                return Ok(customer);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("customers")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomersAsync()
        {
            var customers = await _queries.GetCustomers();
            return Ok(customers);
        }

        [Route("facilities/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(FacilityViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetFacilitiesAsync(int facilityId)
        {
            try
            {
                var facility = await _queries.GetFacility(facilityId);
                return Ok(facility);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("facilites")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FacilityViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FacilityViewModel>>> GetFacilitesAsync()
        {
            var facilities = await _queries.GetFacilities();
            return Ok(facilities);
        }

        [Route("services/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetServiceAsync(int serviceId)
        {
            try
            {
                var service = await _queries.GetService(serviceId);
                return Ok(service);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("services")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ServiceViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ServiceViewModel>>> GetServicesAsync()
        {
            var services = await _queries.GetServices();
            return Ok(services);
        }
    }
}
