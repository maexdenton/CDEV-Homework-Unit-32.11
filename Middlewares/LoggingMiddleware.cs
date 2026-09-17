using System.Diagnostics;
using System.Text;
using WebApplication1.Models.DB;
using WebApplication1.Repositories;

namespace WebApplication1.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        // Семафор для потокобезопасной записи в файл
        // private static readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);
        
        // Объект блокировки для корректной смены цветов в консоли между параллельными потоками
        private static readonly object _consoleLock = new object();

        public LoggingMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        // Внедряем ILogRepository в InvokeAsync
        public async Task InvokeAsync(HttpContext context, ILogRepository logRepository)
        {
            var watch = Stopwatch.StartNew();

            // Передаем запрос дальше по конвейеру
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

            // Вывод в консоль (красивый)
            WriteColoredConsoleLog(now.ToString("yyyy-MM-dd HH:mm:ss.fff"), environment, clientIp, method, fullPath, statusCode, elapsedMs);

            // Безопасная фоновая запись в файл (больше не пишем логи в файл, т.к. у нас теперь есть БД для этого)
            // string plainLogMessage = $"[{now:yyyy-MM-dd HH:mm:ss.fff}] [{environment}] [IP: {clientIp}] {method} {fullPath} - Статус: {statusCode} ({elapsedMs} мс)";
            // await WriteToFileAsync(plainLogMessage);

            // Сохраняем лог в БД
            try
            {
                await logRepository.AddLogAsync(logEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА ЗАПИСИ ЛОГА В БД]: {ex.Message}");
            }
        }

        // Метод для потокобезопасного цветного вывода в консоль
        private void WriteColoredConsoleLog(
            string timestamp,
            string environment,
            string clientIp,
            string method,
            string path,
            int statusCode,
            long elapsedMs)
        {
            lock (_consoleLock)  // Блокировка гарантирует, что цвета не перемешаются при параллельных запросах
            {
                // Время - Темно-серый
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{timestamp}] ");

                // Среда - Голубой
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"[{environment}] ");

                // IP - Темно-желтый
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"[IP: {clientIp}] ");

                // HTTP Метод - Цвет зависит от типа запроса
                Console.ForegroundColor = method switch
                {
                    "GET" => ConsoleColor.Cyan,
                    "POST" => ConsoleColor.Yellow,
                    "PUT" => ConsoleColor.Magenta,
                    "DELETE" => ConsoleColor.Red,
                    _ => ConsoleColor.White
                };
                Console.Write($"{method} ");

                // Путь - Белый
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{path} - ");

                // Статус ответа - подсветка успехов и ошибок
                Console.ForegroundColor = statusCode switch
                {
                    >= 200 and < 300 => ConsoleColor.Green,    // Успех (зеленый)
                    >= 300 and < 400 => ConsoleColor.Cyan,     // Перенаправление (голубой)
                    >= 400 and < 500 => ConsoleColor.Yellow,   // Ошибка клиента (желтый)
                    >= 500 => ConsoleColor.Red,                // Ошибка сервера (красный)
                    _ => ConsoleColor.Gray
                };
                Console.Write($"Статус: {statusCode} ");

                // Время отклика - Темно-серый
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"({elapsedMs} мс)");

                // Сброс цвета консоли к системному по умолчанию
                Console.ResetColor();
            }
        }

        /*
        // Асинхронная запись лога в файл
        private async Task WriteToFileAsync(string logMessage)
        {
            try
            {
                string logsFolderPath = Path.Combine(_env.ContentRootPath, "Logs");

                if (!Directory.Exists(logsFolderPath))
                {
                    Directory.CreateDirectory(logsFolderPath);
                }

                string fileName = $"log-{DateTime.Now:yyyy-MM-dd}.txt";
                string filePath = Path.Combine(logsFolderPath, fileName);

                await _fileLock.WaitAsync();
                try
                {
                    await File.AppendAllTextAsync(filePath, logMessage + Environment.NewLine, Encoding.UTF8);
                }
                finally
                {
                    _fileLock.Release();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА ЗАПИСИ В ФАЙЛ]: {ex.Message}");
            }
        }
        */
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
