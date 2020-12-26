namespace Services.Customer.API.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Customer.API.Application.Commands;
    using Services.Customer.API.Application.Queries;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    [Route("api/{controller}")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICustomerQueries _queries;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IMediator mediator, ICustomerQueries queries, ILogger<CustomerController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerViewModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(CustomerViewModel), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CustomerViewModel>> Create([FromBody] string name)
        {
            var command = new CreateCustomerCommand(name);

            _logger.LogInformation(
                "----- Sending command: {CommandName}: {CommandId} ({@Command})",
                typeof(CreateCustomerCommand).Name,
                nameof(command.Name),
                command.Name,
                command);

            var customerId = await _mediator.Send(command);

            var customer = _queries.GetAsync(customerId);

            return Created("", customer);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] Guid customerId, [FromBody] string name)
        {
            var command = new UpdateCustomerCommand(name, customerId);

            _logger.LogInformation(
                "----- Sending command: {CommandName}: {CommandId} ({@Command})",
                typeof(UpdateCustomerCommand).Name,
                nameof(command.Id),
                command.Id,
                command);

            var result = await _mediator.Send(command);

            if (result)
                return Ok();
            else
                return BadRequest();

        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromBody] Guid customerId)
        {
            var command = new DeleteCustomerCommand(customerId);

            _logger.LogInformation(
                "----- Sending command: {CommandName}: {CommandId} ({@Command})",
                typeof(DeleteCustomerCommand).Name,
                nameof(command.Id),
                command.Id,
                command);

            var result = await _mediator.Send(command);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<CustomerViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<CustomerViewModel>>> Get()
        {
            return Ok(await _queries.GetAsync());
        }
    }
}
