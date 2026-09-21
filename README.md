# BlogPlatform (ASP.NET Core MVC)

Учебный проект веб-приложения на **ASP.NET Core MVC** с использованием **Entity Framework Core**, разработанный в рамках модуля 32.11.

## 🛠 Стек технологий
- **.NET 8 / C#**
- **ASP.NET Core MVC**
- **Entity Framework Core 8** (MS SQL Server Express)
- **Bootstrap 5 & JS (Fetch API / AJAX)**

## 📌 Что реализовано в проекте

### 1. Модели и Автоматизация Базы Данных (`Models/DB`)
- Сущности: `User`, `UserPost` (связь 1-to-Many), `Feedback`, `RequestLog`.
- **Автоматическое создание БД и миграции:** При старте приложения Entity Framework Core автоматически проверяет наличие MS SQL Server Express и создает базу данных (`BlogPlatformDb`) со всеми таблицами через `EnsureCreated()` / миграции — ручной прогон CLI-команд не требуется.

### 2. Архитектура (Repository + Service Layer)
- **Слой репозиториев:** `UserRepository`, `UserPostRepository`, `FeedbackRepository`, `LogRepository`.
- **Сервисный слой:** `FeedbackService` изолирует бизнес-логику от контроллеров (принцип "тонких" контроллеров).

### 3. Представления и Фронтенд
- `/Users` — Просмотр авторов и регистрация нового пользователя.
- `/Feedback` — Отправка отзывов через **AJAX (Fetch API)** без перезагрузки страницы (`wwwroot/js/feedback.js`).
- `/about` — Страница "О нас".

### 4. Кастомное логирование (`LoggingMiddleware`)
- Перехват всех HTTP-запросов и сохранение в БД (`RequestLogs`).
- Вывод через структурированный `ILogger`.
- Фильтрация служебного спама EF Core в `appsettings.Development.json`.

### 5. Работа с окружениями (`Development` / `Production`)
- Страница логов `/logs` и кнопка в меню доступны **только в режиме `Development`** (в `Production` возвращает 404).

## 🚀 Запуск проекта
1. Убедитесь, что запущен локальный **MS SQL Server Express** (`.\SQLEXPRESS`). База данных развернется автоматически.
2. Проверьте строку подключения в `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.\\SQLEXPRESS;Database=BlogPlatformDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

### Запуск в режиме разработки (Development):
```bash
dotnet run --environment Development
```

### Запуск в боевом режиме (Production):
```bash
dotnet run --environment Production
```
