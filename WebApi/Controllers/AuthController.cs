using DAL.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;
using System.Security.Claims;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("registration")]
    public IActionResult Registration([FromBody] CreationUserDto model)
    {
        // TODO: валидация

        User user = userService.CreateUser(model);

        return Ok(user.Id);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        User? user = userService.GetUserByLogin(model.Login);
        if (user is null || user.Password != model.Password)
        {
            return BadRequest("Неверный логин или пароль");
        }

        var claims = new List<Claim>()
        {
            new("user_id", user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        return SignIn(principal, null, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout() => SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
}