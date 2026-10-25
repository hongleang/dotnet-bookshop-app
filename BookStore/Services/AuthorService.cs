using BookStore.Data;
using BookStore.Data.DTOs;
using BookStore.Data.DTOs.Author;
using BookStore.Enums;
using BookStore.Models.Entities;
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
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Bio = a.Bio,
                DateOfBirth = a.DateOfBirth
            })
            .ToListAsync();
    }

    public async Task<AuthorDto?> GetAuthorById(int id)
    {
        return await _db.Authors
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Bio = a.Bio,
                DateOfBirth = a.DateOfBirth,
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AuthorDto> CreateAuthor(CreateAuthorDto requestDto)
    {
        var author = new Author
        {
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            Bio = requestDto.Bio,
            DateOfBirth = requestDto.DateOfBirth
        };
        
        _db.Authors.Add(author);
        await _db.SaveChangesAsync();
        
        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Bio = author.Bio,
            DateOfBirth = author.DateOfBirth
        };
    }
    
    public async Task<AuthorDto?> UpdateAuthor(int id, UpdateAuthorDto dto)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);

        if (author is null) return null;
        
        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;
        author.Bio = dto.Bio;
        author.DateOfBirth = dto.DateOfBirth;
        
        await _db.SaveChangesAsync();
        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Bio = author.Bio,
            DateOfBirth = author.DateOfBirth
        };
    }

    public async Task<DeleteAuthorResult> DeleteAuthor(int id)
    {
        var row = await _db.Authors.Where(a => a.Id == id)
            .Select(a => new { Author = a, HasBooks = a.Books.Any(b => !b.IsDeleted) })
            .FirstOrDefaultAsync();
        
        if (row is null) return DeleteAuthorResult.NotFound;

        if (row.HasBooks) return DeleteAuthorResult.HasBooks;
        
        _db.Authors.Remove(row.Author);
        await _db.SaveChangesAsync();
        
        return DeleteAuthorResult.Deleted;
    }

    public async Task<bool> AuthorExists(int id)
    {
        return await _db.Authors.AnyAsync(a => a.Id == id);
    }
}