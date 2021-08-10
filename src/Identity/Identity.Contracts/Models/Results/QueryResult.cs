namespace Services.Identity.Contracts.Models.Results
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
