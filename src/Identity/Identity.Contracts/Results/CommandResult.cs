namespace Services.Identity.Contracts.Results
{
    using System;

    public class CommandResult : Result
    {
        public CommandResult() : base()
        {
        }

        public CommandResult(string message) : base(message)
        {
        }

        public CommandResult(Exception exception) : base(exception)
        {
        }
    }
}
