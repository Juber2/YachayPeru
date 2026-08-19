using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace YachayPeru.Application.Common.Behaviors
{
    public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var name = typeof(TRequest).Name;
            var sw = Stopwatch.StartNew();

            _logger.LogInformation("Executing {RequestName}", name);

            try
            {
                var response = await next();
                _logger.LogInformation("Completed {RequestName} in {ElapsedMs}ms", name, sw.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed {RequestName} in {ElapsedMs}ms", name, sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
