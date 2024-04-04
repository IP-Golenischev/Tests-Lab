using Grpc.Core;
using Grpc.Core.Interceptors;

namespace HW4.WebHost.Interceptors
{
	public class LogInterceptor(ILogger<LogInterceptor> logger) : Interceptor
	{
		private readonly object _lock = new();

		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			lock (_lock)
			{
				logger.LogInformation("The method {Method} is called with request {request}",
					context.Method, request);
			}

			var response = await continuation(request, context);

			lock (_lock)
			{
				logger.LogInformation("The method {Method} is completed with response {response}",
					context.Method, response);
			}

			return response;
		}
	}
}
