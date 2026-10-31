using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.Data.DTOs.Book;
using BookStore.Enums;
using BookStore.Services.Interfaces;
using BookStore.Validators;
using FluentValidation;

namespace BookStore.Controllers.Api
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IValidator<CreateBookDto> _createValidator;
        private readonly IValidator<UpdateBookDto> _updateValidator;

        public BookController(IValidator<CreateBookDto> createValidator,  IValidator<UpdateBookDto> updateValidator)
        {
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookListDto>>> GetBooks(IBookService service) => Ok(
            await service.GetAllBooks()
        );

        // GET: api/book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id,  IBookService service)
        {
            var book = await service.GetBookById(id);

            if (book is null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/book/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> PutBook(int id, UpdateBookDto requestDto, IBookService service)
        {
            var validation = await _updateValidator.ValidateAsync(requestDto);
            
            if (!validation.IsValid) return ValidationProblem(validation.ToProblemDetails());
            var book = await service.UpdateBook(id, requestDto);

            if (book is null)
            {
                return NotFound();
            }
            
            return Ok(book);
        }

        // POST: api/book
        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBook(CreateBookDto requestDto, IBookService service)
        {
            var validation = await _createValidator.ValidateAsync(requestDto);
            
            if (!validation.IsValid) return ValidationProblem(validation.ToProblemDetails());
            
            var book = await service.CreateBook(requestDto);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id, IBookService service)
        {
            var deleted = await service.DeleteBook(id);
            switch (deleted)
            {
                case DeleteResult.NotFound:
                    return NotFound();
                case DeleteResult.HasBooks:
                    return Conflict(new { message = $"Can't delete authors that have books." });
                default:
                    return NoContent();
            }
        }
    }
}