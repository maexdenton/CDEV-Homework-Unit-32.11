using Microsoft.EntityFrameworkCore;
using BlogPlatform.Data;
using BlogPlatform.Middleware;
using BlogPlatform.Repositories;
using BlogPlatform.Services;

var builder = WebApplication.CreateBuilder(args);

// Подключение DbContext 
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPostRepository, UserPostRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

// Регистрация сервисов бизнес-логики
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Подключение кастомного LoggingMiddleware в начало конвейера
app.UseLoggingMiddleware();

// Инициализация БД в зависимости от окружения
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

    if (app.Environment.IsDevelopment())
    {
        // В режиме разработки автоматически создаем БД и таблицы, если их нет
        dbContext.Database.EnsureCreated();
    }
    else
    {
        // В режиме Production применяем миграции (безопасно для существующих данных)
        // dbContext.Database.Migrate(); 
        dbContext.Database.EnsureCreated(); // Если миграции еще не настроены
    }
}

// Настройка конвейера обработки ошибок и безопасности
if (app.Environment.IsDevelopment())
{
    // В Dev используем подробную страницу ошибок разработчика
    app.UseDeveloperExceptionPage();
}
else
{
    // В Production скрываем стек-трейсы, показываем красивую страницу /Home/Error
    app.UseExceptionHandler("/Home/Error");

    // Включаем HSTS (HTTP Strict Transport Security) для защиты соединения
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();