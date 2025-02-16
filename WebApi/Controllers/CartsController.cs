using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;
using WebApi.Utils;
using WebApi.ViewModel;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
[Authorize(Roles = nameof(UserRole.Customer))]
public class CartsController(ICartService cartService) : ControllerBase
{
    [HttpGet("my")]
    public IActionResult GetOwnerCard()
    {
        User user = HttpContext.GetAuthorizedUser(includeCard: true);
        Cart cart = user.Customer!.Cart;
        return Ok(new CartViewModel(cart));
    }

    [HttpPatch("my")]
    public IActionResult ChangeItemsCard(ChangingCartItemDto changingCardItem)
    {
        User user = HttpContext.GetAuthorizedUser(false, true);
        Cart cart = user.Customer!.Cart;

        changingCardItem.Cart = cart;

        cartService.ChangeRangeItem(changingCardItem);

        return NoContent();
    }

    [HttpPost("my/{cardItemId}/count")]
    public IActionResult IncItemsCard([FromRoute] Guid cardItemId, [FromForm] int count)
    {
        if (!cartService.TryChangeCardItemCount(cardItemId, count)) return BadRequest();

        return NoContent();
    }

    [HttpDelete("my/{cardItemId}/")]
    public IActionResult DeleteItemsCard([FromRoute] Guid cardItemId)
    {
        if (!cartService.TryDeleteCardItem(cardItemId)) return BadRequest();

        return NoContent();
    }
}