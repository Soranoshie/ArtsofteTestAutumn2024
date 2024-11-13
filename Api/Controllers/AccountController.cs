using System.Security.Claims;
using Logic.DAL.Models;
using Logic.Modules.UserModule;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly IUserService userService;

    public AccountController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);
        
        var result = await userService.Login(request, HttpContext);
        
        if (result.Result is OkResult)
            return RedirectToAction("Index", "Cabinet");
        
        if (result is null)
            ModelState.AddModelError("Phone", "Пользователь с таким номером не найден");
        else if (result is InvalidOperationException)
            ModelState.AddModelError("Password", "Неверный пароль");
        
        return View(request);
        
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await userService.SignOutAsync(HttpContext);
        return RedirectToAction("Login", "Account");
    }
    
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await userService.Register(request);
        if (result.Result is BadRequestObjectResult)
        {
            ModelState.AddModelError(string.Empty, "Пользователь с таким номером или почтой уже существует");
            return View(request);
        }

        return RedirectToAction("RegistrationSuccess", new { FIO = request.FIO });
    }
    
    [HttpGet("registration-success")]
    public IActionResult RegistrationSuccess(string FIO)
    {
        ViewData["Message"] = $"Поздравляем {FIO}, вы стали пользователем системы!";
        return View();
    }
}
