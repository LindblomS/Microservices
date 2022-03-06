namespace Ordering.Application.Commands;

using System;
using System.Collections.Generic;

public record CreateOrder(
        Guid UserId,
        string Username,
        CreateOrder.AddressDto Address,
        CreateOrder.CardDto Card,
        IEnumerable<CreateOrder.OrderItem> OrderItems) : Command<bool>
{
    public record CardDto(
        int TypeId,
        string Number,
        string HolderName,
        string SecurityNumber,
        DateTime ExpirationDate);

    public record AddressDto(
        string City,
        string Street,
        string Country,
        string ZipCode,
        string State);

    public record OrderItem(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Units);
}
