using DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;
using System.Security.Claims;
using WebApi.Models;
using WebApi.Utils;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class AuthController(
    IUserService userService,
    IValidator<UserDto> validator
) : ControllerBase
{
    [HttpPost("registration")]
    public IActionResult Registration([FromBody] UserDto model)
    {
        var results = validator.Validate(model);
        if (!results.IsValid)
            return BadRequest(results.Errors
                .Select(e => e.ErrorMessage)
                .ToArray()
            );

        User user = userService.CreateUser(model);

        //todo: добавить редирект
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

    [Authorize]
    [HttpGet("userInfo")]
    public IActionResult GetUserInfo()
    {
        Guid userId = HttpContext.GetAuthorizedUserId();    
        User user = userService.GetUserById(userId)!; 
        return Ok(new {user.Login, RoleName = user.Role.ToString() }) ;
    }
}