using DAL;
using DAL.Entities;
using FluentValidation;
using Services.Dtos;

namespace WebApi.Validators;

public class UpdatingUserDtoValidator : AbstractValidator<UpdatingUserDto>
{
    public UpdatingUserDtoValidator(IUnitOfWork uow)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        When(x => !string.IsNullOrWhiteSpace(x.Login),
            () => RuleFor(x => x.Login)
                .Must(x => ValidateLogin(uow, x)).WithMessage("This login has already existed")
        );

        RuleFor(x => x.Role).IsInEnum();

        RuleFor(x => x.Address).NotEmpty()
            .When(x => x.Role == UserRole.Customer);
        RuleFor(x => x.Name).NotEmpty()
            .When(x => x.Role == UserRole.Customer);

        RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);

        When(x => !string.IsNullOrWhiteSpace(x.Code),
            () => RuleFor(x => x.Code)
                .Matches(@"\d{4}-\d{4}").WithMessage("Code must be format xxxx-yyyy")
                .Must(x => !uow.CustomerRepository.HasCustomerByCode(x))
                .WithMessage("This code has already exist")
        );
    }

    protected virtual bool ValidateLogin(IUnitOfWork uow, string login)
    {
        var user = uow.UserRepository.GetUserByLogin(login);
        return user is null;
    }
}