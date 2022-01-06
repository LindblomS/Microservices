namespace Ordering.Infrastructure.Mappers;

using Ordering.Domain.AggregateModels.Buyer;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.Models;
using System;
using System.Collections.Generic;

internal static class BuyerMapper
{
    public static Buyer Map(BuyerEntity entity, Guid orderId)
    {
        var buyer = new Buyer(Map(entity.Id), entity.Name);

        if (entity.Cards != null)
            foreach (var card in entity.Cards)
                buyer.VerifyOrAddCard(Map(card), orderId);

        buyer.ClearDomainEvents();
        return buyer;
    }

    public static Card Map(CardEntity entity)
    {
        return new Card(
            Map(entity.Id),
            Enumeration.FromValue<CardType>(entity.Type),
            entity.Number,
            entity.SecurityNumber,
            entity.HolderName,
            entity.Expiration);
    }

    public static BuyerEntity Map(Buyer buyer)
    {
        return new BuyerEntity
        {
            Id = Map(buyer.Id),
            Name = buyer.Name,
            Cards = Map(buyer.Cards, buyer.Id)
        };
    }

    static List<CardEntity> Map(IEnumerable<Card> cards, Guid buyerId)
    {
        var list = new List<CardEntity>();

        foreach (var card in cards)
            list.Add(Map(card, buyerId));

        return list;
    }

    static CardEntity Map(Card card, Guid buyerId)
    {
        return new CardEntity
        {
            Id = Map(card.Id),
            BuyerId = Map(buyerId),
            Type = card.Type.Id,
            Number = card.Number,
            SecurityNumber = card.SecurityNumber,
            HolderName = card.HolderName,
            Expiration = card.Expiration
        };
    }

    static string Map(Guid value)
    {
        return value == default ? null : value.ToString();
    }

    static Guid Map(string value)
    {
        return value is null ? Guid.Empty : Guid.Parse(value);
    }
}
