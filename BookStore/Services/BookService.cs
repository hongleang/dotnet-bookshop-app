using BookStore.Data;
using BookStore.Data.DTOs;
using BookStore.Data.DTOs.Book;
using BookStore.Enums;
using BookStore.Mappings;
using BookStore.Models.Entities;
using BookStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _db;

    public BookService(AppDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<IReadOnlyCollection<BookListDto>> GetAllBooks()
    {
        return await _db.Books
            .AsNoTracking()
            .ProjectToBookListDto()
            .ToListAsync();
    }

    public async Task<BookDto?> GetBookById(int id)
    {
        return await _db.Books
            .AsNoTracking()
            .Where(b => b.Id == id)
            .ProjectToBookDto()
            .AsSplitQuery()
            .FirstOrDefaultAsync();
    }

    public async Task<BookDto> CreateBook(CreateBookDto requestDto)
    {
        var book = new Book
        {
            Title = requestDto.Title,
            PublishedDate = requestDto.PublishedDate,
            Price = requestDto.Price,
            TotalCopies = requestDto.TotalCopies,
            AvailableCopies = requestDto.AvailableCopies,
            AuthorId = requestDto.AuthorId,
            BookDetail = new BookDetail
            {
                Isbn = requestDto.Isbn,
                Pages = requestDto.Pages,
                Publisher = requestDto.Publisher,
                Language = requestDto.Language,
                Description = requestDto.Description,
            }
        };

        var requested = requestDto.Categories ?? [];

        var newCategories = requested
            .Where(c => c.Id is null && !string.IsNullOrWhiteSpace(c.Name))
            .Select(c => c.Name!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var existing = requested
            .Where(c => c.Id is not null && !string.IsNullOrWhiteSpace(c.Name))
            .Select(c => c.Name!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var allRequested = newCategories.Concat(existing).ToList();

        var records = await _db.Categories
            .Where(c => allRequested.Contains(c.Name))
            .ToListAsync();

        var toCreate = newCategories
            .Where(n => !records.Any(e => string.Equals(e.Name, n, StringComparison.OrdinalIgnoreCase)))
            .Select(n => new Category { Name = n })
            .ToList();

        foreach (var category in records.Concat(toCreate))
        {
            book.Categories.Add(category);
        }

        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Price = book.Price,
            Isbn = book.BookDetail.Isbn,
            Pages = book.BookDetail.Pages,
            Publisher = book.BookDetail.Publisher,
            AuthorId = book.AuthorId,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            Language = book.BookDetail.Language,
            Description = book.BookDetail.Description,
            Categories = book.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList(),
            PublishedDate = default
        };
    }

    public async Task<BookDto?> UpdateBook(int id, UpdateBookDto requestDto)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return null;

        book.Title = requestDto.Title;
        book.PublishedDate = requestDto.PublishedDate;
        book.Price = requestDto.Price;
        book.TotalCopies = requestDto.TotalCopies;
        book.AvailableCopies = requestDto.AvailableCopies;
        book.AuthorId = requestDto.AuthorId;
        book.BookDetail = new BookDetail
        {
            Isbn = requestDto.Isbn,
            Pages = requestDto.Pages,
            Publisher = requestDto.Publisher,
            Language = requestDto.Language,
            Description = requestDto.Description,
        };

        await _db.SaveChangesAsync();
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Price = book.Price,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            Isbn = book.BookDetail.Isbn,
            Pages = book.BookDetail.Pages,
            Publisher = book.BookDetail.Publisher,
            AuthorId = book.AuthorId,
            PublishedDate = default,
        };
    }

    public async Task<DeleteResult> DeleteBook(int id)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return DeleteResult.NotFound;

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();

        return DeleteResult.Deleted;
    }
}