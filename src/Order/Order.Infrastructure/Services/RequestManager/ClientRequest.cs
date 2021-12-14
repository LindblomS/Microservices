namespace Ordering.Infrastructure.Services.RequestManager;

using System;

internal class ClientRequest
{
    public ClientRequest(Guid id, string name, DateTime time)
    {
        Id = id;
        Name = name;
        Time = time;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
}
