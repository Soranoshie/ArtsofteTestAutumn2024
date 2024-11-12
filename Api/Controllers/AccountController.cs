using Logic.DAL.Models;
using Logic.Modules.UserModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Properties.Controllers;

[Route("account")]
//[Area("Account")]
public class AccountController : Controller
{
    private readonly IUserService userService;

    public AccountController(IUserService userService)
    {
        this.userService = userService;
    }

    // Страница авторизации (GET)
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View("Views/Account/Login.cshtml"); // Возвращаем форму авторизации
    }

    // Обработка авторизации (POST)
    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request); // Если валидация не прошла, возвращаем форму с ошибками
        }

        var result = await userService.Login(request, HttpContext);
        if (result.Result is BadRequestObjectResult || result.Result is NotFoundObjectResult)
        {
            ModelState.AddModelError(string.Empty, "Ошибка авторизации"); // Добавляем ошибку на все поля
            return View(request); // Возвращаем форму с ошибкой
        }

        // Успешная авторизация, перенаправляем на кабинет
        return RedirectToAction("Index", "Cabinet");
    }

    // Страница выхода
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await userService.SignOutAsync(HttpContext);
        return RedirectToAction("Login", "Account");
    }

    // Страница кабинета (защищенная)
    [Authorize] // Доступ только для авторизованных пользователей
    [HttpGet("cabinet")]
    public IActionResult Cabinet()
    {
        return RedirectToAction("Index", "Cabinet"); // Редирект на кабинет
    }

    // Страница регистрации
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View(); // Возвращаем форму регистрации
    }

    // Обработка регистрации
    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await userService.Register(request);
        if (result.Result is BadRequestObjectResult)
        {
            return View(request); // Ошибка при регистрации
        }

        return RedirectToAction("RegistrationSuccess", new { FIO = request.FIO });
    }

    // Страница успешной регистрации
    [HttpGet("registration-success")]
    public IActionResult RegistrationSuccess(string FIO)
    {
        ViewBag.FIO = FIO;
        return View();
    }
}