namespace BookStore.Data.DTOs;

public record CategoryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
