namespace Services.Identity.Infrastructure.Idempotency
{
    using System;

    public class ClientRequest
    {
        public ClientRequest(Guid id, string name, DateTime time)
        {
            
            Id = id != default ? id : throw new ArgumentNullException(nameof(id));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Time = time != default ? time : throw new ArgumentNullException(nameof(time));
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTime Time { get; private set; }
    }
}
