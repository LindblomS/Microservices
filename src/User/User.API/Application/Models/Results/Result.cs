namespace Services.User.API.Application.Models.Results
{
    using System;

    public abstract class Result
    {
        protected Result()
        {
            Success = true;
        }

        protected Result(string message)
        {
            Success = false;
            FailureMessage = message;
        }

        protected Result(Exception exception)
        {
            Success = false;
            Exception = exception;
        }

        public bool Success { get; private set; }
        public string FailureMessage { get; private set; }
        public Exception Exception { get; private set; }

        public bool IsFailure()
        {
            return !string.IsNullOrEmpty(FailureMessage);
        }

        public bool IsException()
        {
            return Exception != null;
        }

    }
}
