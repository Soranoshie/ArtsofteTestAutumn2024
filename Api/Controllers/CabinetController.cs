using Logic.Modules.UserModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("cabinet")]
public class CabinetController : Controller
{
    private readonly IUserService userService;

    public CabinetController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userResult = await userService.GetUserMvc(HttpContext);
        if (userResult is UnauthorizedResult || userResult is NotFoundResult)
        {
            return RedirectToAction("Login", "Account");
        }
        
        return View(userResult);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await userService.SignOutAsync(HttpContext);
        return RedirectToAction("Login", "Account");
    }
}