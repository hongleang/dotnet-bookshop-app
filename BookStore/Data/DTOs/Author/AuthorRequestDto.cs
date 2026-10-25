using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.DTOs.Author;

public record AuthorRequestDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Bio { get; init; }
    public DateOnly? DateOfBirth { get; init; }
}

public record CreateAuthorDto: AuthorRequestDto;

public record UpdateAuthorDto: CreateAuthorDto;