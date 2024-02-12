using Microsoft.EntityFrameworkCore;
using MvcStartApp.Models.Db;
using MvcStartApp.Models.Repositories;

namespace MvcStartApp.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        private void LogConsole(HttpContext context)
        {
            // Для логирования данных о запросе используем свойста объекта HttpContext
            Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
        }

        private async Task LogFile(HttpContext context)
        {
            // Строка для публикации в лог
            string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

            // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "RequestLog.txt");

            // Используем асинхронную запись в файл
            await File.AppendAllTextAsync(logFilePath, logMessage);
        }

        private async Task LogToDb(HttpContext context)
        {
            string logMessage = $"http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

            var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Database=master;Integrated Security=True;TrustServerCertificate=True;");

            using (var db = new BlogContext(optionsBuilder.Options))
            {
                RequestRepository requestRepository = new RequestRepository(db);
                await requestRepository.AddAsync(new Request
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Url = logMessage
                });
            }
        }


        public async Task InvokeAsync(HttpContext context)
        {
            LogConsole(context);
            await LogFile(context);
            await LogToDb(context);

            // Передача запроса далее по конвейеру
            await _next.Invoke(context);
        }
    }
}
