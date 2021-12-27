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
        var buyer = new Buyer(entity.Id, entity.Name);

        foreach (var card in entity.Cards)
            buyer.VerifyOrAddCard(Map(card), orderId);

        buyer.ClearDomainEvents();
        return buyer;
    }

    public static Card Map(CardEntity entity)
    {
        return new Card(
            entity.Id,
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
            Id = buyer.Id,
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
            Id = card.Id,
            BuyerId = buyerId,
            Type = card.Type.Id,
            Number = card.Number,
            SecurityNumber = card.SecurityNumber,
            HolderName = card.HolderName,
            Expiration = card.Expiration
        };
    }
}
