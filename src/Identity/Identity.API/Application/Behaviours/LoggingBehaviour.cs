namespace Identity.API.Application.Behaviours
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoggingBehaviour<T, R> : IPipelineBehavior<T, R>
    {
        private readonly ILogger<LoggingBehaviour<T, R>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<T, R>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<R> Handle(T request, CancellationToken cancellationToken, RequestHandlerDelegate<R> next)
        {
            var typeName = typeof(T).Name;
            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", typeName, request);
            var response = await next();
            _logger.LogInformation("----- Command {CommandName} handled - response: {@Response}", typeName, response);

            return response;
        }
    }
}
