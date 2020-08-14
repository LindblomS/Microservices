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
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IQueries _queries;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IMediator mediator, IQueries queries, ILogger<CustomersController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CustomerViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await _queries.GetCustomer(id);
                return Ok(customer);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomersAsync()
        {
            var customers = await _queries.GetCustomers();
            return Ok(customers);
        }

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
    }
}
