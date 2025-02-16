using FluentValidation;
using Services.Dtos;

namespace WebApi.Validators;

public static class DI
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
        services.AddScoped<IValidator<UpdatingUserDto>, UpdatingUserDtoValidator>();

        services.AddScoped<IValidator<ItemDto>, ItemDtoValidator>();
        services.AddScoped<IValidator<UpdatingItemDto>, UpdatingItemDtoValidator>();

        services.AddScoped<IValidator<CreatingOrderDto>, CreationOrderDtoValidator>();
    }
}