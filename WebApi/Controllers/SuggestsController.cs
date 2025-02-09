using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
[Authorize]
public class SuggestsController(IItemService itemService) : ControllerBase
{
    [HttpGet("category")]
    public IActionResult GetCategorySuggest([FromQuery] string query)
    {
        var suggets = itemService.GetCategorySuggest(query);
        return Ok(suggets);
    }
}