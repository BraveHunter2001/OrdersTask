using DAL;
using FluentValidation;
using Services.Dtos;

namespace WebApi.Validators;

public class UpdatingItemDtoValidator : AbstractValidator<UpdatingItemDto>
{
    public UpdatingItemDtoValidator(IUnitOfWork uow)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        When(x => x.Price.HasValue,
            () => RuleFor(x => x.Price).GreaterThanOrEqualTo(0)
        );

        When(x => !string.IsNullOrWhiteSpace(x.Code),
            () => RuleFor(x => x.Code)
                .Matches(@"\d{2}-\d{4}-[A-Z]{2}\d{2}").WithMessage("Code must be format XX-XXXX-YYXX")
                .Must(x => uow.ItemRepository.GetItemByCode(x) is null)
        );
    }
}