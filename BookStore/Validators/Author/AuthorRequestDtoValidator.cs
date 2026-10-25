using BookStore.Data.DTOs.Author;
using FluentValidation;

namespace BookStore.Validators.Author;

public class AuthorRequestDtoValidator<T> : AbstractValidator<T> where T : AuthorRequestDto
{
    public AuthorRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Bio)
            .MaximumLength(2000);

        RuleFor(x => x.DateOfBirth)
            .Must(d => d < DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth must be in the past.")
            .When(x => x.DateOfBirth.HasValue);
    }
}

public class CreateAuthorDtoValidator : AuthorRequestDtoValidator<CreateAuthorDto>
{
};

public class UpdateAuthorDtoValidator : AuthorRequestDtoValidator<UpdateAuthorDto>
{
};