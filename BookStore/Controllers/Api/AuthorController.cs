using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.Data.DTOs.Author;
using BookStore.Enums;
using BookStore.Services.Interfaces;
using BookStore.Validators;
using FluentValidation;

namespace BookStore.Controllers.Api
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IValidator<CreateAuthorDto> _createValidator;
        private readonly IValidator<UpdateAuthorDto> _updateValidator;

        public AuthorController(IValidator<CreateAuthorDto> createValidator,  IValidator<UpdateAuthorDto> updateValidator)
        {
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(IAuthorService service) => Ok(
            await service.GetAllAuthors()
        );

        // GET: api/author/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id,  IAuthorService service)
        {
            var author = await service.GetAuthorById(id);

            if (author is null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // PUT: api/author/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDto>> PutAuthor(int id, UpdateAuthorDto requestDto, IAuthorService service)
        {
            var validation = await _updateValidator.ValidateAsync(requestDto);
            
            if (!validation.IsValid) return ValidationProblem(validation.ToProblemDetails());
            var author = await service.UpdateAuthor(id, requestDto);

            if (author is null)
            {
                return NotFound();
            }
            
            return Ok(author);
        }

        // POST: api/author
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(CreateAuthorDto requestDto, IAuthorService service)
        {
            var validation = await _createValidator.ValidateAsync(requestDto);
            
            if (!validation.IsValid) return ValidationProblem(validation.ToProblemDetails());
            
            var author = await service.CreateAuthor(requestDto);

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // DELETE: api/author/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id, IAuthorService service)
        {
            var deleted = await service.DeleteAuthor(id);

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