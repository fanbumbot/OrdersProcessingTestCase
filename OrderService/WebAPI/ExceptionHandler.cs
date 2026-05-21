using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using FluentValidation;

namespace OrderService.WebAPI
{
    /// <summary>
    /// Обработчик исключений (Middleware)
    /// </summary>
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Метод-обработчик исключений
        /// </summary>
        /// <param name="httpContext">Контекст HTTP/REST</param>
        /// <param name="exception">Информация об исключении</param>
        /// <param name="cancellationToken">Переменная для прерывания задачи</param>
        /// <returns>Всегда возвращает true</returns>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title, logLevel) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found", LogLevel.Warning),
                ValidationException => (StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", LogLevel.Warning),

                _ => (StatusCodes.Status500InternalServerError, "Server Error", LogLevel.Error)
            };

            if (logLevel == LogLevel.Error)
            {
                _logger.Log(logLevel, exception, "Error occurred while processing request: {Message}", exception.Message);
            }
            else if(logLevel == LogLevel.Warning)
            {
                _logger.Log(logLevel, "Error occurred while processing request: {Message}", exception.Message);
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
