namespace Services.User.API.Application.Factories
{
    using Services.User.API.Application.Models.Results;
    using System;


    public static class ResultFactory
    {
        public static T CreateFailureResult<T>(string message) where T : Result
        {
            return (T)Create(typeof(T), message);
        }

        public static T CreateExceptionResult<T>(Exception exception) where T : Result
        {
            return (T)Create(typeof(T), exception);
        }

        public static QueryResult<T> CreateSuccessResult<T>(T result)
        {
            return new QueryResult<T>(result);
        }

        public static CommandResult CreateSuccessResult()
        {
            return new CommandResult();
        }

        private static Result Create(Type type, object value)
        {
            if (type.IsGenericType)
            {
                var resultType = type.GetGenericArguments()[0];
                var result = typeof(QueryResult<>).MakeGenericType(resultType);
                return (Result)Activator.CreateInstance(resultType, value);
            }

            return (Result)Activator.CreateInstance(type, value);
        }
    }
}
