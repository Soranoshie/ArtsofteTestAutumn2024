using Logic.DAL.Models;
using Logic.Modules.UserModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Properties.Controllers;

[Route("api/account")]
[ApiController]
public class AccountApiController : ControllerBase
{
    private readonly IUserService userService;

    public AccountApiController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost("register")]
    public Task<ActionResult<RegisterRequest>> Register([FromBody] RegisterRequest request)
        => userService.Register(request);

    [HttpPost("login")]
    public Task<ActionResult<LoginRequest>> Login([FromBody] LoginRequest request)
        => userService.Login(request, HttpContext);

    [Authorize]
    [HttpGet("get-my-info")]
    public async Task<ActionResult<UserInfoRequest>> GetMyInfo()
        => Ok(await userService.GetUser(HttpContext));

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await userService.SignOutAsync(HttpContext);
        return Ok("Вы успешно вышли из системы");
    }
}