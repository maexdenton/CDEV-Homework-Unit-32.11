using WebApplication1.Middleware;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Регистрация DbContext для работы с SQL Server
            builder.Services.AddDbContext<BlogDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Регистрация репозиториев
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserPostRepository, UserPostRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Подключаем наш кастомный Middleware (ставим его в начало, чтобы оно замеряло время работы всех последующих компонентов)
            app.UseLoggingMiddleware();

            // Автоматическое создание БД SQL Server (если она еще не создана)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
                dbContext.Database.EnsureCreated(); // Создает БД и таблицы на основе моделей
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Перенаправляет HTTP-запросы на защищенное HTTPS-соединени
            app.UseHttpsRedirection();

            // Middleware для обслуживания статических файлов из каталога wwwroot: CSS, JavaScript, изображения и шрифты
            app.UseStaticFiles();

            // Middleware маршрутизации.
            // Определяет, какой обработчик должен обслужить входящий запрос
            app.UseRouting();

            // Middleware авторизации.
            // Проверяет, имеет ли пользователь права на доступ к ресурсу
            app.UseAuthorization();

            // Регистрация маршрута для статических ресурсов приложения
            app.MapStaticAssets();

            // Middleware, связывающее контроллеры и маршруты
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
