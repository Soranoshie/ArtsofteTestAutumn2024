using Logic.DAL.Entities;
using Logic.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logic.Modules.UserModule;

public interface IUserService
{
    public Task<ActionResult<RegisterRequest>> Register(RegisterRequest request);
    Task<ActionResult<LoginRequest>> Login(LoginRequest request, HttpContext httpContext);
    public Task<ActionResult> SignOutAsync(HttpContext httpContext);
    Task<ActionResult<IEnumerable<UserEntity>>> GetUsers();
    Task<ActionResult<UserEntity>> GetUser(HttpContext httpContext);
    Task<ActionResult<UserEntity>> CreateOrUpdateUser(UserEntity userEntity);
    Task<ActionResult> DeleteUser(string Email);
}