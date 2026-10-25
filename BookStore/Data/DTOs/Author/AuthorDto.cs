namespace BookStore.Data.DTOs.Author;

public record AuthorDto
{
    public required int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Bio { get; init; }
    public DateOnly? DateOfBirth { get; init; }
}
