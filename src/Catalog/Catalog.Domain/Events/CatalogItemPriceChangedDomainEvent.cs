namespace Catalog.Domain.Events;

using MediatR;
using System;

public record CatalogItemPriceChangedDomainEvent(Guid Id, decimal NewPrice) : INotification;
