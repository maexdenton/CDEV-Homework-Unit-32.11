using System.Diagnostics;
using WebApplication1.Models.DB;
using WebApplication1.Repositories;

namespace WebApplication1.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<LoggingMiddleware> _logger; // Внедряем ILogger

        public LoggingMiddleware(
            RequestDelegate next,
            IWebHostEnvironment env,
            ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ILogRepository logRepository)
        {
            var watch = Stopwatch.StartNew();

            await _next(context);

            watch.Stop();

            // Собираем данные события
            DateTime now = DateTime.Now;
            string method = context.Request.Method;
            string fullPath = $"{context.Request.Path}{context.Request.QueryString}";
            int statusCode = context.Response.StatusCode;
            string environment = _env.EnvironmentName;
            string clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            long elapsedMs = watch.ElapsedMilliseconds;

            var logEntity = new RequestLog
            {
                Timestamp = now,
                Environment = environment,
                Method = method,
                Path = fullPath,
                StatusCode = statusCode,
                ElapsedMs = elapsedMs,
                ClientIp = clientIp
            };

            // Логирование через ILogger (структурированное логирование)
            // Определяем уровень лога в зависимости от статуса ответа
            LogLevel logLevel = statusCode switch
            {
                >= 500 => LogLevel.Error,       // Ошибки сервера (5xx)
                >= 400 => LogLevel.Warning,     // Ошибки клиента (4xx)
                _ => LogLevel.Information       // Успешные запросы (2xx, 3xx)
            };

            // Шаблон со структурированными параметрами {Environment}, {ClientIp}, {Method} и т.д.
            _logger.Log(
                logLevel,
                "[{Environment}] [IP: {ClientIp}] {Method} {Path} - Статус: {StatusCode} ({ElapsedMs} мс)",
                environment,
                clientIp,
                method,
                fullPath,
                statusCode,
                elapsedMs
            );

            // Сохранение лога в БД
            try
            {
                await logRepository.AddLogAsync(logEntity);
            }
            catch (Exception ex)
            {
                // Ошибки записи в БД также логируем через ILogger
                _logger.LogError(ex, "Ошибка при сохранении HTTP-лога в базу данных.");
            }
        }
    }

    // Метод расширения для удобного подключения в Program.cs
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}