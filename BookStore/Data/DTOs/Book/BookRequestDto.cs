namespace BookStore.Data.DTOs.Book;

public record BookRequestDto
{
    public required string Title { get; init; }
    public required DateOnly PublishDate { get; init; }
    public required decimal Price { get; init; }
    public required int TotalCopies { get; init; }
    public required int AvailableCopies { get; init; }
    public required int AuthorId { get; init; }
    public required string Isbn { get; init; }
    public required int Pages { get; init; }
    public required string Publisher { get; init; }
    public string? Language { get; init; }
    public string? Description { get; init; }
    public IEnumerable<BookCategoryDto>? Categories { get; init; }
}

public record BookCategoryDto
{
    public int? Id { get; init; }
    public string? Name { get; init; }
}

public record CreateBookDto : BookRequestDto;

public record UpdateBookDto : BookRequestDto;