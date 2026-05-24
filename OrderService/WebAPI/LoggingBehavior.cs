namespace OrderService.WebAPI
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics;
    using System.Text.Json;

    /// <summary>
    /// Логирование действий для контроллеров
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Обработка вызовов
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="next">Следующий обработчик</param>
        /// <param name="cancellationToken">Переменная для отмены задачи</param>
        /// <returns>Результат выполнения запроса</returns>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            var stopwatch = Stopwatch.StartNew();

            var response = await next();
            stopwatch.Stop();

            var requestData = JsonSerializer.Serialize(request);
            _logger.LogInformation("[Request] {RequestName} ({Elapsed}ms). Data: {Data}",
                requestName, stopwatch.ElapsedMilliseconds, requestData);

            return response;
        }
    }
}
