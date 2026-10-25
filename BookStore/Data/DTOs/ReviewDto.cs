namespace BookStore.Data.DTOs;

public record ReviewDto
{
    public required int Id { get; init; }
    public required int BookId { get; init; }
    public required int MemberId { get; init; }
    public required decimal Rating { get; init; }
    public string? Comment { get; init; }
}
