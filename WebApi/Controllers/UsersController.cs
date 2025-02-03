using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost("registration")]
    public IActionResult Registration([FromBody] CreationUserDto model)
    {
        // валидация

        //
        User user = userService.CreateUser(model);

        return Ok(user.Id);
    }


}