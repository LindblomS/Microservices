namespace Ordering.Application.Commands;

public record SetStockRejectedOrderStatus(Guid OrderId, IEnumerable<Guid> StockItems) : Command<bool>;
