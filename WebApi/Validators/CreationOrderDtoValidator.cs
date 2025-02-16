using FluentValidation;
using Services.Dtos;

namespace WebApi.Validators;

public class CreationOrderDtoValidator : AbstractValidator<CreatingOrderDto>
{
    public CreationOrderDtoValidator()
    {
        RuleForEach(x=>x.Items)
            .NotNull()
            .Must(oi=>oi.Count > 0)
            .WithMessage("Items in order must be more zero");
    }
}
