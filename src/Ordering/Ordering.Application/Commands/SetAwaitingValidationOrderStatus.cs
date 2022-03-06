namespace Ordering.Application.Commands;

public record SetAwaitingValidationOrderStatus(Guid OrderId) : Command<bool>;
