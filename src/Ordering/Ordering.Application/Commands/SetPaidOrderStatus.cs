namespace Ordering.Application.Commands;

public record SetPaidOrderStatus(Guid OrderId) : Command<bool>;