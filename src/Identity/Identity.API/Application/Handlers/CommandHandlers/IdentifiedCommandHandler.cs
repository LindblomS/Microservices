namespace Identity.API.Application.Handlers.CommandHandlers
{
    using MediatR;
    using Services.Identity.Contracts.Commands;
    using Services.Identity.Infrastructure.Idempotency;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R> where T : IRequest<R>
    {
        private readonly IRequestManager _requestManager;
        private readonly IMediator _mediator;

        public IdentifiedCommandHandler(IRequestManager requestManager, IMediator mediator)
        {
            _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<R> Handle(IdentifiedCommand<T, R> request, CancellationToken cancellationToken)
        {
            var exists = await _requestManager.ExistAsync(request.Id);

            if (exists)
                return CreateResultForDuplicateRequest();

            await _requestManager.CreateRequestForCommandAsync<T>(request.Id);
            return await _mediator.Send(request.Command, cancellationToken);
        }

        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }
    }
}
