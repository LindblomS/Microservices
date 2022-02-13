namespace Catalog.API.Filters;

using Catalog.API.ActionResults;
using Catalog.Application.Exceptions;
using Catalog.Domain.Exceptions;
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

        if (context.Exception.GetType() == typeof(CatalogDomainException))
        {
            HandleCatalogDomainException(context, (CatalogDomainException)context.Exception);
            return;
        }

        if (context.Exception.GetType() == typeof(CommandValidationException))
        {
            HandleCommandValidationException(context, (CommandValidationException)context.Exception);
            return;
        }

        HandleInternalServerError(context);
        
    }

    void HandleInternalServerError(ExceptionContext context)
    {
        var response = new JsonErrorResponse
        {
            Messages = new[] { "Internal server error" },
            DeveloperMessage = environment.IsDevelopment() ? context.Exception.ToString() : ""
        };

        context.Result = new InternalServerErrorObjectResult(response);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }

    void HandleCatalogDomainException(ExceptionContext context, CatalogDomainException exception)
    {
        var details = new ValidationProblemDetails
        {
            Instance = context.HttpContext.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Detail = "Domain error. Please refer to the errors property for additional details"
        };

        details.Errors.Add("Validations", new string[] { context.Exception.Message });
        context.Result = new BadRequestObjectResult(details);
        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.ExceptionHandled = true;
    }

    void HandleCommandValidationException(ExceptionContext context, CommandValidationException exception)
    {
        var details = new ValidationProblemDetails
        {
            Instance = context.HttpContext.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Detail = "Validation error. Please refer to the errors property for additional details"
        };

        details.Errors.Add("Validations", exception.ValidationErrors.ToArray());
        context.Result = new BadRequestObjectResult(details);
        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.ExceptionHandled = true;
    }

    class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public string DeveloperMessage { get; set; }
    }
}
