namespace Catalog.API.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        throw new NotImplementedException();
    }
}
