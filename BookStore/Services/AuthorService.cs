using BookStore.Data;
using BookStore.Data.DTOs.Author;
using BookStore.Enums;
using BookStore.Mappings;
using BookStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public class AuthorService: IAuthorService
{
    private readonly AppDbContext _db;
    
    public AuthorService(AppDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public async Task<IReadOnlyCollection<AuthorDto>> GetAllAuthors()
    {
        return await _db.Authors
            .AsNoTracking()
            .ProjectToAuthorDto()
            .ToListAsync();
    }

    public async Task<AuthorDto?> GetAuthorById(int id)
    {
        return await _db.Authors
            .AsNoTracking()
            .Where(a => a.Id == id)
            .ProjectToAuthorDto()
            .FirstOrDefaultAsync();
    }

    public async Task<AuthorDto> CreateAuthor(CreateAuthorDto requestDto)
    {
        var author = requestDto.CreateAuthorDtoToEntity();
        
        _db.Authors.Add(author);
        await _db.SaveChangesAsync();

        return author.ToAuthorDto();
    }
    
    public async Task<AuthorDto?> UpdateAuthor(int id, UpdateAuthorDto dto)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);

        if (author is null) return null;
        
        dto.UpdateAuthorEntity(author);
        
        await _db.SaveChangesAsync();
        
        return author.ToAuthorDto();
    }

    public async Task<DeleteResult> DeleteAuthor(int id)
    {
        var row = await _db.Authors.Where(a => a.Id == id)
            .Select(a => new { Author = a, HasBooks = a.Books.Any(b => !b.IsDeleted) })
            .FirstOrDefaultAsync();
        
        if (row is null) return DeleteResult.NotFound;

        if (row.HasBooks) return DeleteResult.HasBooks;
        
        _db.Authors.Remove(row.Author);
        await _db.SaveChangesAsync();
        
        return DeleteResult.Deleted;
    }
}