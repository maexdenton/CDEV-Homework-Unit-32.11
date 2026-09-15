using System.Diagnostics;

namespace WebApplication1.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        // Семафор для потокобезопасной записи нескольких одновременных запросов в один файл
        private static readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env)
        {
            var watch = Stopwatch.StartNew();

            // Выполнение следующих компонентов конвейера (обработка запроса)
            await _next(context);

            watch.Stop();

            // Формируем единую строку события с текущей меткой времени DateTime.Now
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string method = context.Request.Method;
            string path = context.Request.Path;
            int statusCode = context.Response.StatusCode;
            string environment = env.EnvironmentName;

            string logMessage = $"[{timestamp}] [{environment}] {method} {path} - Статус: {statusCode} ({watch.ElapsedMilliseconds} мс)";

            // Пишем в консоль
            Console.WriteLine(logMessage);

            // Пишем в файл
            await WriteToFileAsync(env, logMessage);
        }

        private async Task WriteToFileAsync(IWebHostEnvironment env, string logMessage)
        {
            try
            {
                // Путь к папке Logs в корне проекта
                string logsFolderPath = Path.Combine(env.ContentRootPath, "Logs");

                if (!Directory.Exists(logsFolderPath))
                {
                    Directory.CreateDirectory(logsFolderPath);
                }

                // Создаем имя файла с текущей датой (например, log-2026-03-30.txt)
                string fileName = $"log-{DateTime.Now:yyyy-MM-dd}.txt";
                string filePath = Path.Combine(logsFolderPath, fileName);

                // Безопасная асинхронная дозапись в файл
                await _fileLock.WaitAsync();
                try
                {
                    await File.AppendAllTextAsync(filePath, logMessage + Environment.NewLine);
                }
                finally
                {
                    _fileLock.Release();
                }
            }
            catch (Exception ex)
            {
                // Если не удалось записать в файл, сообщаем об этом в консоль
                Console.WriteLine($"[ОШИБКА ЗАПИСИ ЛОГА В ФАЙЛ]: {ex.Message}");
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
