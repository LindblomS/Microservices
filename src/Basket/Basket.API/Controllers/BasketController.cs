namespace Basket.API.Controllers;

using Basket.API.Mappers;
using Basket.Contracts.IntegrationEvents;
using Basket.Domain.AggregateModels;
using EventBus.EventBus.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    readonly IBasketRepository basketRepository;
    readonly ILogger<BasketController> logger;
    readonly IEventBus eventBus;

    public BasketController(
        IBasketRepository basketRepository,
        ILogger<BasketController> logger,
        IEventBus eventBus)
    {
        this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    [Route("{buyerId}")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Contracts.Models.BasketItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Contracts.Models.BasketItem>>> GetAsync(string buyerId)
    {
        if (!Guid.TryParse(buyerId, out var parsedId))
            return BadRequest("Invalid buyer id. Buyer id must be a valid GUID");

        var basket = await basketRepository.GetBasketAsync(parsedId);

        if (basket is null)
            return BadRequest($"Basket with buyer id {parsedId} does not exists");

        return Ok(BasketMapper.Map(basket.Items));
    }

    [Route("{buyerId}")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(string buyerId)
    {
        if (!Guid.TryParse(buyerId, out var parsedId))
            return BadRequest("Buyer id was invalid. Buyer id must be a valid GUID");

        logger.LogInformation("Deleting basket with buyer id {BuyerId}", parsedId);
        await basketRepository.DeleteBasketAsync(parsedId);

        return Ok();
    }

    [Route("checkout")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCheckout([FromBody] Contracts.Models.BasketCheckout checkout, [FromHeader(Name = "request_id")] string requestId)
    {
        if (!Guid.TryParse(checkout.BuyerId, out var buyerId))
            return BadRequest("Buyer id was invalid. Buyer id must be a valid GUID");

        if (!Guid.TryParse(requestId, out var parsedRequestId))
            return BadRequest("Request id was invalid. Request id must be a valid GUID");

        if (checkout.Address is null)
            return BadRequest("Address was missing");

        if (checkout.Card is null)
            return BadRequest("Card was missing");

        if (string.IsNullOrWhiteSpace(checkout.Username))
            return BadRequest("Username was empty");

        var basket = await basketRepository.GetBasketAsync(buyerId);

        if (basket is null)
            return BadRequest($"Basket with buyer id {buyerId} does not exists");

        var integrationEvent = new UserCheckoutAcceptedIntegrationEvent(
            buyerId,
            checkout.Username,
            parsedRequestId,
            BasketMapper.Map(checkout.Address),
            BasketMapper.Map(checkout.Card),
            basket.Items.Select(x => BasketMapper.MapEventItem(x)));

        try
        {
            logger.LogInformation("Publishing integration event: {IntegrationId} from Catalog", integrationEvent.Id);
            eventBus.Publish(integrationEvent);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error publishing integration event: {IntegrationEventId} from Catalog", integrationEvent.Id);
            throw;
        }

        return Accepted();
    }

    [Route("{buyerId}")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAsync(string buyerId, [FromBody] string productId)
    {
        if (!Guid.TryParse(buyerId, out var parsedBuyerId))
            return BadRequest("Buyer id was invalid. Buyer id must be a valid GUID");


    }

    [Route("{buyerId}")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(string buyerId, [FromBody] IEnumerable<Guid> items)
    {
        if (!Guid.TryParse(buyerId, out var parsedBuyerId))
            return BadRequest("Buyer id was invalid. Buyer id must be a valid GUID");

        var originalBasket = await basketRepository.GetBasketAsync(parsedBuyerId);
        var originalItems = originalBasket is null 
            ? new List<BasketItem>() 
            : originalBasket.Items.ToList();

        var basket = new Basket(parsedBuyerId);
        foreach (var item in items)
        {
            if (!Guid.TryParse(item.ProductId, out var parsedProductId))
                return BadRequest("ProductId was not valid for one or more items");

            var oldPrice = originalItems.SingleOrDefault(x => x.ProductId == parsedProductId)?.OldUnitPrice ?? 0;
            basket.AddBasketItem(BasketMapper.Map(item, oldPrice));
        }

        logger.LogInformation("Updating basket with buyerId {BuyerId}", parsedBuyerId);
        await basketRepository.CreateUpdateBasketAsync(basket);
        return Ok();
    }
}
