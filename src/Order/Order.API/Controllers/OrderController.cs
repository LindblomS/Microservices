namespace Services.Order.API.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Order.API.Application.Commands.Commands;
    using Services.Order.API.Application.Queries;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    [Route("api/{controller}")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueries _queries;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, IOrderQueries queries, ILogger<OrderController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName}: ({@Command})",
                typeof(CreateOrderCommand).Name,
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
        public async Task<IActionResult> Delete([FromBody] DeleteOrderCommand command)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName}: ({@Command})",
                typeof(DeleteOrderCommand).Name,
                command);

            var result = await _mediator.Send(command);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> Get()
        {
            return Ok(await _queries.GetOrdersAsync());
        }
    }
}
