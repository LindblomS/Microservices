namespace Basket.API.Filters;

using Basket.API.ActionResults;
using Basket.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionFilter : IExceptionFilter
{
    readonly ILogger<ExceptionFilter> logger;
    readonly IWebHostEnvironment environment;

    public ExceptionFilter(ILogger<ExceptionFilter> logger, IWebHostEnvironment environment)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public void OnException(ExceptionContext context)
    {
        logger.LogError(
            new EventId(context.Exception.HResult),
            context.Exception,
            context.Exception.Message);

        if (context.Exception.GetType() == typeof(BasketDomainException))
        {
            var details = new ValidationProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details"
            };

            details.Errors.Add("Validations", new string[] { context.Exception.Message });
            context.Result = new BadRequestObjectResult(details);
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.ExceptionHandled = true;
            return;
        }

        var response = new JsonErrorResponse
        {
            Messages = new[] { "Internal server error" },
            DeveloperMessage = environment.IsDevelopment() ? context.Exception : ""
        };

        context.Result = new InternalServerErrorObjectResult(response);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }

    class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMessage { get; set; }
    }
}
