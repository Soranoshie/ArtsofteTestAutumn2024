using Logic.Modules.UserModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;

namespace Api.Properties.Controllers;

[Route("cabinet")]
//[Area("Cabinet")]
public class CabinetController : Controller
{
    private readonly IUserService userService;

    public CabinetController(IUserService userService)
    {
        this.userService = userService;
    }

    // Страница с детальной информацией о пользователе
    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userResult = await userService.GetUser(HttpContext);
        if (userResult is UnauthorizedResult || userResult is NotFoundResult)
        {
            return RedirectToAction("Login", "Account");
        }

        var user = userResult.Value;  // Получаем объект пользователя
        return View(user);  // Передаем пользователя в представление
    }

    // Страница выхода
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await userService.SignOutAsync(HttpContext);  // Разлогиниваем пользователя
        return RedirectToAction("Login", "Account"); // Редиректим на страницу авторизации
    }
}