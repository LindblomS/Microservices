namespace Services.Identity.Contracts.Results
{
    public class QueryResult<T> : Result
    {
        public QueryResult(T result) : base()
        {
            Result = result;
        }

        public T Result { get; private set; }
    }
}
