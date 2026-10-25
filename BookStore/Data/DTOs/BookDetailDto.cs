namespace BookStore.Data.DTOs;

public record BookDetailDto
{
    public required int Id { get; init; }
    public required string Isbn { get; init; }
    public required int Pages { get; init; }
    public required string Publisher { get; init; }
    public string? Language { get; init; }
    public string? Description { get; init; }
}
