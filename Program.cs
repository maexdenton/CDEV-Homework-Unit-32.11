using WebApplication1.Middleware;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Подключаем наш кастомный Middleware (ставим его в начало, чтобы оно замеряло время работы всех последующих компонентов)
            app.UseLoggingMiddleware();

            // Перенаправляет HTTP-запросы на защищенное HTTPS-соединени
            app.UseHttpsRedirection();

            // Middleware для обслуживания статических файлов
            // из каталога wwwroot: CSS, JavaScript, изображения и шрифты
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
