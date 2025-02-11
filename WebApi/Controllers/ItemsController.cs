using DAL.Dto;
using DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;
using WebApi.Validators;
using WebApi.ViewModel;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ItemsController(
    IItemService itemService,
    IValidator<ItemDto> itemDtoValidator,
    IValidator<UpdatingItemDto> updatingItemDtoValidator
) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Manager))]
    public IActionResult CreateItem(ItemDto itemDto)
    {
        var validationResults = itemDtoValidator.ValidateModel(itemDto);
        if (validationResults is not null) return validationResults;

        Item item = itemService.CreateItem(itemDto);

        return Ok(item);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = nameof(UserRole.Manager))]
    public IActionResult UpdateItem([FromRoute] Guid id, UpdatingItemDto model)
    {
        Item? item = itemService.GetItemById(id);
        if (item is null)
            return NotFound("This item dont exist");

        var validationResults = updatingItemDtoValidator.ValidateModel(model);
        if (validationResults is not null) return validationResults;

        itemService.UpdateItem(item, model);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRole.Manager))]
    public IActionResult DeleteItem([FromRoute] Guid id)
    {
        Item? item = itemService.GetItemById(id);
        if (item is null)
            return NotFound("This item dont exist");

        itemService.DeleteItem(item);
        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult GetItem([FromRoute] Guid id) => Ok(itemService.GetItemById(id));

    [HttpGet("paginated")]
    [Authorize]
    public IActionResult GetPaginatedOrder([FromQuery] ItemListFilter filter)
    {
        var paginatedList = itemService.GetPaginatedItemList(filter);

        var paginatedListVieModal = new PaginatedContainer<List<ItemListViewModel>>(
            paginatedList.Value.ConvertAll(i => new ItemListViewModel(i)),
            paginatedList.TotalCount,
            paginatedList.TotalPages
        );
        return Ok(paginatedListVieModal);
    }
}