namespace Ordering.Infrastructure.Models;

using System;

public class ClientRequestEntity
{
    public ClientRequestEntity(string id, string name, DateTime time)
    {
        Id = id;
        Name = name;
        Time = time;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
}
