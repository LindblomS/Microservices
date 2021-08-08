﻿namespace Services.Identity.Infrastructure.Models
{
    using MediatR;
    using System.Collections.Generic;

    public record Command(string Sql, object Parameters, IEnumerable<INotification> Notifications);
}