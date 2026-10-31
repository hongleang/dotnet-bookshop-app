using BookStore.Data.DTOs.Book;
using BookStore.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace BookStore.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class BookMapper
{
    [MapProperty("BookDetail.Isbn", nameof(BookDto.Isbn))]
    [MapProperty("BookDetail.Pages", nameof(BookDto.Pages))]
    [MapProperty("BookDetail.Publisher", nameof(BookDto.Publisher))]
    [MapProperty("BookDetail.Language", nameof(BookDto.Language))]
    [MapProperty("BookDetail.Description", nameof(BookDto.Description))]
    public static partial BookDto ToBookDto(this Book book);
    
    public static partial IQueryable<BookListDto> ProjectToBookListDto(this IQueryable<Book> books);
    public static partial IQueryable<BookDto> ProjectToBookDto(this IQueryable<Book> books);
    
    [MapperIgnoreTarget(nameof(Book.Id))]
    [MapperIgnoreTarget(nameof(Book.Author))]
    [MapperIgnoreTarget(nameof(Book.Loans))]
    [MapperIgnoreTarget(nameof(Book.Reviews))]
    [MapperIgnoreTarget(nameof(Book.IsDeleted))]
    [MapperIgnoreTarget(nameof(Book.BookDetail))]
    [MapperIgnoreTarget(nameof(Book.Categories))]
    public static partial Book CreateBookDtoToEntity(this CreateBookDto dto);
    
    [MapperIgnoreTarget(nameof(Book.Id))]
    [MapperIgnoreTarget(nameof(Book.Author))]
    [MapperIgnoreTarget(nameof(Book.Loans))]
    [MapperIgnoreTarget(nameof(Book.Reviews))]
    [MapperIgnoreTarget(nameof(Book.IsDeleted))]
    [MapperIgnoreTarget(nameof(Book.BookDetail))]
    [MapperIgnoreTarget(nameof(Book.Categories))]
    public static partial void UpdateBookEntity(this UpdateBookDto dto, [MappingTarget] Book book);
}