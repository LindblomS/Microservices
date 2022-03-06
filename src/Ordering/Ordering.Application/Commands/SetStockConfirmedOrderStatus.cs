namespace Ordering.Application.Commands;

public record SetStockConfirmedOrderStatus(Guid OrderId) : Command<bool>;
