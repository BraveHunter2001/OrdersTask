using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;
using WebApi.Utils;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IUserService userService, IOrderService orderService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public IActionResult CreateOrder([FromBody] CreationOrderDto creationOrderDto)
    {
        var userId = HttpContext.GetAuthorizedUserId();
        User user = userService.GetUserById(userId);

        if (user is null || user is not Customer customer)
            return BadRequest("Not corrected user");

        var order = orderService.CreateOrder(customer, creationOrderDto);

        return Ok(new { order.Id, order.Status });
    }
}