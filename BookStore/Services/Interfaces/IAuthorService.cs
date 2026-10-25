using BookStore.Data.DTOs;
using BookStore.Data.DTOs.Author;
using BookStore.Enums;

namespace BookStore.Services.Interfaces;

public interface IAuthorService
{
    // Crud service
    Task<IReadOnlyCollection<AuthorDto>> GetAllAuthors();
    Task<AuthorDto?> GetAuthorById(int id);
    Task<AuthorDto> CreateAuthor(CreateAuthorDto requestDto);
    Task<AuthorDto?> UpdateAuthor(int id, UpdateAuthorDto dto);
    Task<DeleteAuthorResult> DeleteAuthor(int id);
    Task<bool> AuthorExists(int id);
}