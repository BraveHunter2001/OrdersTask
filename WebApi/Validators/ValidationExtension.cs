using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Validators;

public static class ValidationExtension
{
    public static IActionResult? ValidateModel<T>(this IValidator<T> validator, T model)
    {
        var results = validator.Validate(model);
        if (!results.IsValid)
            return new BadRequestObjectResult(results.Errors
                .Select(e => e.ErrorMessage)
                .ToArray()
            );

        return null;
    }
}