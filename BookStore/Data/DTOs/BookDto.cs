namespace BookStore.Data.DTOs;

public record BookDto
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required DateOnly PublishDate { get; init; }
    public required decimal Price { get; init; }
    public int? TotalCopies { get; init; }
    public int? AvailableCopies { get; init; }
    public required int? AuthorId { get; init; }
    public BookDetailDto? Detail { get; init; }
    public IReadOnlyCollection<CategoryDto>? Categories { get; init; }
    public IReadOnlyCollection<LoanDto>? Loans { get; init; }
    public IReadOnlyCollection<ReviewDto>? Reviews { get; init; }
}
