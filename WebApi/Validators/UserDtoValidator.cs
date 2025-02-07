using DAL;
using DAL.Entities;
using FluentValidation;
using Services.Dtos;

namespace WebApi.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator(IUnitOfWork uow)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Login).NotEmpty()
            .Must(x => !ValidateLogin(uow, x)).WithMessage("This login has already existed");
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Role).IsInEnum();

        RuleFor(x => x.Address).NotEmpty()
            .When(x => x.Role == UserRole.Customer);

        RuleFor(x => x.Name).NotEmpty()
            .When(x => x.Role == UserRole.Customer);
    }

    protected virtual bool ValidateLogin(IUnitOfWork uow, string login)
    {
        var user = uow.UserRepository.GetUserByLogin(login);
        return user is not null;
    }
}