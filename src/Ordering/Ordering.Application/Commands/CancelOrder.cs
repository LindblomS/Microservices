namespace Ordering.Application.Commands;

using System;

public record CancelOrder(Guid OrderId) : Command<bool>;
