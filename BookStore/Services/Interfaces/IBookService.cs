using BookStore.Data.DTOs;
using BookStore.Data.DTOs.Book;
using BookStore.Enums;

namespace BookStore.Services.Interfaces;

public interface IBookService
{
    // Crud service
    Task<IReadOnlyCollection<BookListDto>> GetAllBooks();
    Task<BookDto?> GetBookById(int id);
    Task<BookDto> CreateBook(CreateBookDto requestDto);
    Task<BookDto?> UpdateBook(int id, UpdateBookDto dto);
    Task<DeleteResult> DeleteBook(int id);
}