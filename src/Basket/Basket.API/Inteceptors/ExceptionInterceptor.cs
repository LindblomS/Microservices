namespace Basket.API.Inteceptors;

using Grpc.Core;
using Grpc.Core.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    readonly ILogger<ExceptionInterceptor> logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            return continuation(request, context);
        }
        catch (RpcException exception) when (exception.StatusCode == StatusCode.InvalidArgument)
        {
            logger.LogWarning("{StatusCode} - {Message}", nameof(StatusCode.InvalidArgument), exception.Message);
            throw;
        }
        catch (RpcException exception) when (exception.StatusCode == StatusCode.NotFound)
        {
            logger.LogWarning("{StatusCode} - {Message}", nameof(StatusCode.NotFound), exception.Message);
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{StatusCode} - {Message}", nameof(StatusCode.Internal), exception.Message);
            throw new RpcException(new(StatusCode.Internal, "Internal server error"));
        }
    }
}
