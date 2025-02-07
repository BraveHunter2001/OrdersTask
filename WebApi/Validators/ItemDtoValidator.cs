using FluentValidation;
using Services.Dtos;

namespace WebApi.Validators;

public class ItemDtoValidator : AbstractValidator<ItemDto>
{
    public ItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}