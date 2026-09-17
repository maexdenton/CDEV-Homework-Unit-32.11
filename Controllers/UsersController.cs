using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.DB;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        // Внедряем репозиторий через конструктор
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Метод для получения всех пользователей (доступен по адресу /Users)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            var sortedUsers = users.OrderByDescending(u => u.CreatedAt).ToList();

            return View(sortedUsers);
        }

        // Отображение страницы с формой регистрации
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Обработка данных формы регистрации нового пользователя
        [HttpPost]
        public async Task<IActionResult> Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                newUser.CreatedAt = DateTime.UtcNow;

                // Сохраняем пользователя в БД через репозиторий
                await _userRepository.AddAsync(newUser);

                //Console.WriteLine($"[Успех] Зарегистрирован пользователь {newUser.UserName} (Email: {newUser.Email})");

                // Перенаправляем на страницу со списком пользователей (/Users)
                return RedirectToAction(nameof(Index));
            }

            // Если форма заполнена некорректно, возвращаем ее с ошибками
            return View(newUser);
        }
    }
}
