using Grpc.Core;
using Grpc.Core.Interceptors;
using HW4.Exceptions;

namespace HW4.Interceptors
{
	public class ErrorInterceptor : Interceptor
	{
		private readonly ILogger<LogInterceptor> _logger;

		public ErrorInterceptor(ILogger<LogInterceptor> logger) => _logger = logger;

		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
			ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
		{
			try
			{
				return await continuation(request, context);
			}
			catch (InvalidArgumentException e)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
			}
			catch (AlreadyExistsException e)
			{
				throw new RpcException(new Status(StatusCode.AlreadyExists, e.Message));
			}
			catch (NotFoundException e)
			{
				throw new RpcException(new Status(StatusCode.NotFound, e.Message));
			}
			catch (ArgumentNullException e)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
			}
			catch (Exception e)
			{
				_logger.LogCritical(e, "When executing method {Method} an error occured: {Message}",
					context.Method, e.Message);
				throw new RpcException(new Status(StatusCode.Internal, ""));
			}
		}
	}
}
