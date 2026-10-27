using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.Data.DTOs.Book;
using BookStore.Services.Interfaces;
using BookStore.Validators;
using FluentValidation;

namespace BookStore.Controllers.Api
{
    [Route("api/authors")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<CreateBookDto> _createValidator;
        private readonly IValidator<UpdateBookDto> _updateValidator;

        public BookController(AppDbContext context,  IValidator<CreateBookDto> createValidator,  IValidator<UpdateBookDto> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(IBookService service) => Ok(
            await service.GetAllBooks()
        );

        // GET: api/author/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id,  IBookService service)
        {
            var author = await service.GetBookById(id);

            if (author is null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // PUT: api/author/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> PutBook(int id, UpdateBookDto requestDto, IBookService service)
        {
            var validation = await _updateValidator.ValidateAsync(requestDto);
            
            if (!validation.IsValid) return ValidationProblem(validation.ToProblemDetails());
            var author = await service.UpdateBook(id, requestDto);

            if (author is null)
            {
                return NotFound();
            }
            
            return Ok(author);
        }

        // POST: api/author
        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBook(CreateBookDto requestDto, IBookService service)
        {
            var validation = await _createValidator.ValidateAsync(requestDto);
            
            if (!validation.IsValid) return ValidationProblem(validation.ToProblemDetails());
            
            var author = await service.CreateBook(requestDto);

            return CreatedAtAction(nameof(GetBook), new { id = author.Id }, author);
        }

        // DELETE: api/author/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id, IBookService service)
        {
            var deleted = await service.DeleteBook(id);
            return Ok(deleted);
        }
    }
}