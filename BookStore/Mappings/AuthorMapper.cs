using BookStore.Data.DTOs.Author;
using BookStore.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace BookStore.Mappings;

[Mapper]
public static partial class AuthorMapper
{
    public static partial AuthorDto ToAuthorDto(this Author author);
    
    public static partial Author CreateAuthorDtoToEntity(this CreateAuthorDto dto);
    
    public static partial IQueryable<AuthorDto> ProjectToAuthorDto(this IQueryable<Author> authors);
    
    public static partial void UpdateAuthorEntity(this UpdateAuthorDto dto, [MappingTarget] Author author);
}