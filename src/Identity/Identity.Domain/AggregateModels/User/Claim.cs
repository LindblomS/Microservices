namespace Services.Identity.Domain.AggregateModels.User
{
    using System;

    public class Claim
    {
        public Claim(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            Key = key;
            Value = value;
        }

        public string Key { get; }
        public string Value { get; }
    }
}
