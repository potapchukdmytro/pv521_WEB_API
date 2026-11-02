using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;
using PV521_BooksShop.Dtos;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorRepository _authorRepository;
        private readonly AppDbContext _context;

        public AuthorsController(
            AuthorRepository authorRepository,
            AppDbContext context)
        {
            _authorRepository = authorRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorRepository.Authors
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Biography = a.Biography,
                    Image = a.Image,
                    Country = a.Country,
                    BirthDate = a.BirthDate
                })
                .ToListAsync();

            return Ok(authors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound("Автора не знайдено");
            }

            return Ok(ToDto(author));
        }

        [HttpGet("{id:int}/books")]
        public async Task<IActionResult> GetBooksByAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound("Автора не знайдено");
            }

            var books = await _context.Books
                .AsNoTracking()
                .Where(b => b.AuthorId == id)
                .Select(b => new AuthorBookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Image = b.Image,
                    Rating = b.Rating,
                    Price = b.Price,
                    Pages = b.Pages,
                    Year = b.Year,
                    AuthorId = b.AuthorId
                })
                .ToListAsync();

            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAuthorDto dto)
        {
            var author = new Author
            {
                Name = dto.Name,
                Biography = dto.Biography,
                Image = dto.Image,
                Country = dto.Country,
                BirthDate = dto.BirthDate!.Value.ToUniversalTime()
            };

            await _authorRepository.CreateAsync(author);

            return CreatedAtAction(
                nameof(GetById),
                new { id = author.Id },
                ToDto(author));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateAuthorDto dto)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound("Автора не знайдено");
            }

            author.Name = dto.Name;
            author.Biography = dto.Biography;
            author.Image = dto.Image;
            author.Country = dto.Country;
            author.BirthDate = dto.BirthDate!.Value.ToUniversalTime();

            await _authorRepository.UpdateAsync(author);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _authorRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Автора не знайдено");
            }

            return NoContent();
        }

        private static AuthorDto ToDto(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                Image = author.Image,
                Country = author.Country,
                BirthDate = author.BirthDate
            };
        }
    }
}
