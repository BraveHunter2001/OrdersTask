using DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;
using WebApi.Validators;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
[Authorize(Roles = nameof(UserRole.Manager))]
public class UsersController(
    IUserService userService,
    IValidator<UserDto> userDtoValidator,
    IValidator<UpdatingUserDto> updatingUserModelValidator
) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateUser([FromBody] UserDto model)
    {
        var validationResults = userDtoValidator.ValidateModel(model);
        if (validationResults is not null) return validationResults;

        Func<UserDto, User> create = model.IsCustomer
            ? userService.CreateCustomer
            : userService.CreateUser;

        User user = create(model);
        return Ok(user.Id);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser([FromRoute] Guid id)
    {
        User? user = userService.GetUserById(id);
        if (user is not null)
            return NotFound("This user dont exist");

        userService.DeleteUser(user!);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateUser([FromRoute] Guid id, UpdatingUserDto model)
    {
        User? user = userService.GetUserById(id);
        if (user is null)
            return NotFound("This user dont exist");

        var validationResults = updatingUserModelValidator.ValidateModel(model);
        if (validationResults is not null) return validationResults;

        if (user.Role == UserRole.Customer && user is Customer customer)
            userService.UpdateCustomer(customer, model);
        else
            userService.UpdateUser(user, model);
        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(Guid id) =>
        Ok(userService.GetUserById(id));
}