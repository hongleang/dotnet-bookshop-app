using BookStore.Data.DTOs.Book;
using FluentValidation;

namespace BookStore.Validators.Book;

public class BookRequestDtoValidator<T> : AbstractValidator<T> where T : BookRequestDto
{
    public BookRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .NotNull()
            .GreaterThan(0)
            .PrecisionScale(precision: 10, scale: 2, ignoreTrailingZeros: true);

        RuleFor(x => x.TotalCopies)
            .NotNull()
            .GreaterThan(0);
        
        RuleFor(x => x.AuthorId).NotNull().NotEmpty();
    }
}

public class CreateBookDtoValidator : BookRequestDtoValidator<CreateBookDto>
{
};

public class UpdateBookDtoValidator : BookRequestDtoValidator<UpdateBookDto>
{
};