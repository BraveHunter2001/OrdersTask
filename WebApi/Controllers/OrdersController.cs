using DAL.Dto;
using DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;
using WebApi.Utils;
using WebApi.Validators;
using WebApi.ViewModel;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(
    IUserService userService,
    IOrderService orderService,
    IValidator<CreationOrderDto> creationOrderDtoValidator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Customer))]
    public IActionResult CreateOrder([FromBody] CreationOrderDto creationOrderDto)
    {
        var validationResult = creationOrderDtoValidator.ValidateModel(creationOrderDto);
        if (validationResult is not null) return validationResult;

        var userId = HttpContext.GetAuthorizedUserId();
        User user = userService.GetUserById(userId)!;
        //Todo fix
        var order = orderService.CreateOrder(user.Customer, creationOrderDto);

        return Ok(new { order.Id, order.Status });
    }

    [HttpPost("{id}/accept")]
    [Authorize(Roles = nameof(UserRole.Manager))]
    public IActionResult AcceptOrder([FromRoute] Guid id)
    {
        Order? order = orderService.GetSimpleOrderById(id);
        if (order is null) return NotFound();

        orderService.AcceptOrder(order);

        return NoContent();
    }

    [HttpPost("{id}/close")]
    [Authorize(Roles = nameof(UserRole.Manager))]
    public IActionResult CloseOrder([FromRoute] Guid id)
    {
        Order? order = orderService.GetSimpleOrderById(id);
        if (order is null) return NotFound();

        orderService.CloseOrder(order);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRole.Customer))]
    public IActionResult DeleteOrder([FromRoute] Guid id)
    {
        Order? order = orderService.GetSimpleOrderById(id);
        if (order is null) return NotFound();

        if (order.CustomerId != HttpContext.GetAuthorizedUserId())
            return Forbid("You are not the owner of the order");

        if (!orderService.TryDeleteOrder(order))
            return BadRequest("At this status it is impossible to delete the order");

        return NoContent();
    }

    [HttpGet("paginated")]
    [Authorize]
    public IActionResult GetPaginatedOrder([FromQuery] OrderListFilter filter)
    {
        if (HttpContext.GetAuthorizedUserRole() == UserRole.Customer)
        {
            filter.CustomerId = HttpContext.GetAuthorizedUserId();
        }

        var paginatedList = orderService.GetPaginatedOrderList(filter);

        var paginatedListVieModal = new PaginatedContainer<List<OrderListItemViewModel>>(
            paginatedList.Value.ConvertAll(o => new OrderListItemViewModel(o)),
            paginatedList.TotalCount,
            paginatedList.TotalPages
        );
        return Ok(paginatedListVieModal);
    }
}