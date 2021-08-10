namespace Services.Identity.Domain.ValueObjects
{
    using System;

    public record Claim
    {
        public Claim(string type, string value)
        {
            ValidateType(type);
            ValidateValue(value);

            Type = type;
            Value = value;
        }

        public string Type { get; }
        public string Value { get; }

        private void ValidateType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException(nameof(type));

            if (type.Length > 50)
                throw new ArgumentException("claim type cannot exceede 50 characters");
        }

        private void ValidateValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (value.Length > 50)
                throw new ArgumentException("claim value cannot exceede 50 characters");
        }
    }
}
