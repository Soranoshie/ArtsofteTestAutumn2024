using System.Security.Claims;
using Logic.DAL.Entities;
using Logic.DAL.Models;
using Logic.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logic.Modules.UserModule;

public class UserService : ControllerBase, IUserService
{
    private readonly IUserRepository userRepository;

    public UserService(
        IUserRepository userRepository
    )
    {
        this.userRepository = userRepository;
    }

    public async Task<ActionResult<RegisterRequest>> Register(RegisterRequest request)
    {
        if (userRepository.Any(request))
        {
            var errorResponse = new ErrorResponse
            {
                Code = StatusCodes.Status400BadRequest.ToString(),
                Message = "Пользователь с таким номером или почтой уже существует"
            };

            return BadRequest(errorResponse);
        }

        // у меня куча ошибок из-за любого упоминания AutoMapper
        var user = new UserEntity
        {
            FIO = request.FIO,
            Phone = request.Phone,
            Email = request.Email,
            Password = request.Password,
            LastLogin = DateTime.Now,
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return NoContent();
    }

    public async Task<ActionResult<LoginRequest>> Login(LoginRequest request, HttpContext httpContext)
    {
        var user = await userRepository.FindByPhoneNumberAsync(request.Phone);
        if (user == null)
        {
            var errorResponse = new ErrorResponse
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Message = "Пользователь не найден"
            };
            return NotFound(errorResponse);
        }

        if (request.Password != user.Password)
        {
            var errorResponse = new ErrorResponse
            {
                Code = StatusCodes.Status400BadRequest.ToString(),
                Message = "Неверный пароль"
            };
            return BadRequest(errorResponse);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.MobilePhone, user.Phone)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
            });

        return Ok();
    }

    public async Task<ActionResult> SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }

    public async Task<ActionResult<UserEntity>> GetUser(HttpContext httpContext)
    {
        var email = httpContext.User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
        {
            var errorResponse = new ErrorResponse
            {
                Code = StatusCodes.Status401Unauthorized.ToString(),
                Message = "Пользователь не авторизован"
            };

            return Unauthorized(errorResponse);
        }

        var user = await userRepository.FindByEmailAsync(email);
        Console.WriteLine($"User: {user?.FIO}, {user?.Phone}");
        if (user == null)
        {
            var errorResponse = new ErrorResponse
            {
                Code = StatusCodes.Status404NotFound.ToString(),
                Message = "Пользователь не найден"
            };

            return NotFound(errorResponse);
        }

        return Ok(user);
    }

    public async Task<UserEntity> GetUserMvc(HttpContext httpContext)
    {
        var email = httpContext.User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
            throw new UnauthorizedAccessException("Пользователь не авторизован");

        var user = await userRepository.FindByEmailAsync(email);
        if (user == null)
            throw new KeyNotFoundException("Пользователь не найден");

        return user;
    }

    public Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<UserEntity>> CreateOrUpdateUser(UserEntity userEntity)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> DeleteUser(string Email)
    {
        throw new NotImplementedException();
    }
}