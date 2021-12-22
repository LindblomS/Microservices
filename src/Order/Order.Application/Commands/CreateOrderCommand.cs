namespace Ordering.Application.Commands;

using MediatR;
using System;
using System.Collections.Generic;

public class CreateOrderCommand : IRequest<bool>
{
    public CreateOrderCommand(
        Guid userId,
        string username,
        AddressDto address,
        CardDto card,
        IEnumerable<OrderItem> orderItems)
    {
        UserId = userId;
        Username = username;
        Address = address;
        Card = card;
        OrderItems = orderItems;
    }

    public Guid UserId { get; private set; }
    public string Username { get; private set; }
    public AddressDto Address { get; private set; }
    public CardDto Card { get; private set; }
    public IEnumerable<OrderItem> OrderItems { get; private set; }

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
