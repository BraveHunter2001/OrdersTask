using FluentValidation;
using Services.Dtos;

namespace WebApi.Validators;

public class CreationOrderDtoValidator : AbstractValidator<CreationOrderDto>
{
    public CreationOrderDtoValidator()
    {
        RuleForEach(x=>x.OrderItems)
            .Must(oi=>oi.Count > 0)
            .WithMessage("Items in order must be more zero");
    }
}
