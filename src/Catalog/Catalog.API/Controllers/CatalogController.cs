namespace Catalog.API.Controllers;

using Catalog.API.Mappers;
using Catalog.API.Repositories;
using Catalog.Contracts.IntegrationEvents;
using Catalog.Domain.Aggregates;
using EventBus.EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    readonly ICatalogRepository catalogRepository;
    readonly ICatalogQueryRepository catalogQueryRepository;
    readonly ILogger<CatalogController> logger;
    readonly IEventBus eventBus;

    public CatalogController(
        ICatalogRepository catalogRepository,
        ICatalogQueryRepository catalogQueryRepository,
        ILogger<CatalogController> logger,
        IEventBus eventBus)
    {
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.catalogQueryRepository = catalogQueryRepository ?? throw new ArgumentNullException(nameof(catalogQueryRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    [Route("items")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.CatalogItem>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Models.CatalogItem>> GetItems()
    {
        return Ok(catalogQueryRepository.GetItems());
    }

    [Route("items")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] Models.CatalogItem item)
    {
        logger.LogInformation("Creating catalog item {@CatalogItem}", item);
        await catalogRepository.CreateAsync(CatalogMapper.Map(Guid.NewGuid(), item));
        return Ok();
    }

    [Route("items/{id}")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        if (!Guid.TryParse(id, out var catalogId))
            return BadRequest($"Invalid id. Id must be a valid GUID. Id was {id}");

        logger.LogInformation("Deleting catalog item {CatalogId}", catalogId);
        await catalogRepository.DeleteAsync(catalogId);
        return Ok();
    }

    [Route("items/{id}")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Models.CatalogItem item)
    {
        if (!Guid.TryParse(id, out var catalogId))
            return BadRequest($"Invalid id. Id must be a valid GUID. Id was {id}");

        var original = await catalogRepository.GetAsync(catalogId);

        if (original is null)
            return BadRequest($"Catalog item with id {catalogId} does not exist");

        logger.LogInformation("Updating catalog item {CatalogId} - {@CatalogItem}", catalogId, item);

        var oldPrice = original.Price;

        var updated = CatalogMapper.Map(catalogId, item);
        await catalogRepository.UpdateAsync(updated);

        if (oldPrice != updated.Price)
        {
            var integrationEvent = new ProductPriceChangedIntegrationEvent(catalogId, updated.Price);
            eventBus.Publish(integrationEvent);
        }

        return Ok();
    }
}
