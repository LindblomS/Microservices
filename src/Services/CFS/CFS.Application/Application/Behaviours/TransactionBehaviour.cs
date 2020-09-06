using CFS.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Extensions;

namespace CFS.Application.Application.Behaviours
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IDb _db;
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;

        public TransactionBehaviour(IDb db, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (_db.HasActiveTransaction)
                    return await next();

                using (var transaction = await _db.BeginTransactionAsync())
                {
                    _logger.LogInformation("----- Begin transaction  for {CommandName} ({@Command})", typeName, request);

                    response = await next();

                    _logger.LogInformation("----- Commit transaction for {CommandName}", typeName);

                    await _db.CommitTransactionAsync(transaction);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}
