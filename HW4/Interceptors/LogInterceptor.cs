using Grpc.Core;
using Grpc.Core.Interceptors;

namespace HW4.Interceptors
{
	public class LogInterceptor : Interceptor
	{
		private readonly ILogger<LogInterceptor> _logger;
		private readonly object _lock = new();

		public LogInterceptor(ILogger<LogInterceptor> logger) => _logger = logger;

		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			lock (_lock)
			{
				_logger.LogInformation("The method {Method} is called with request {request}",
					context.Method, request);
			}

			var response = await continuation(request, context);

			lock (_lock)
			{
				_logger.LogInformation("The method {Method} is completed with response {response}",
					context.Method, response);
			}

			return response;
		}
	}
}
