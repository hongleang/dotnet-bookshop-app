using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Validators;

public static class ValidationResultExtensions
{
    public static ValidationProblemDetails ToProblemDetails(this ValidationResult result) =>
        new(result.ToDictionary())
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Validation failed"
        };
}