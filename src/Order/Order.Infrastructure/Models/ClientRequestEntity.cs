namespace Ordering.Infrastructure.Models;

using System;

public class ClientRequestEntity
{
    public ClientRequestEntity(Guid id, string name, DateTime time)
    {
        Id = id;
        Name = name;
        Time = time;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
}
