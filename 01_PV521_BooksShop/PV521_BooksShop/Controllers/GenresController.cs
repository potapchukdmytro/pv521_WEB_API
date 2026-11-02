using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using PV521_BooksShop.DAL;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.Controllers
{
    public class SaveGenreDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        public List<int> BookIds { get; set; } = [];
    }

    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<int> BookIds { get; set; } = [];
    }

    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GenresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _context.Genres
                .AsNoTracking()
                .OrderBy(g => g.Id)
                .Select(g => new GenreDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    BookIds = g.Books
                        .Select(b => b.Id)
                        .ToList()
                })
                .ToListAsync();

            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] SaveGenreDto dto)
        {
            if (dto.BookIds == null)
                return BadRequest("Вкажіть список BookIds");

            var ids = dto.BookIds.Distinct().ToList();

            var books = await _context.Books
                .Where(b => ids.Contains(b.Id))
                .ToListAsync();

            if (books.Count != ids.Count)
                return BadRequest("Деякі книги не знайдені");

            var genre = new Genre
            {
                Name = dto.Name.Trim(),
                Books = books
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return StatusCode(
                StatusCodes.Status201Created,
                ToDto(genre));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] SaveGenreDto dto)
        {
            var genre = await _context.Genres
                .Include(g => g.Books)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                return NotFound("Жанр не знайдено");

            if (dto.BookIds == null)
                return BadRequest("Вкажіть список BookIds");

            var ids = dto.BookIds.Distinct().ToList();

            var books = await _context.Books
                .Where(b => ids.Contains(b.Id))
                .ToListAsync();

            if (books.Count != ids.Count)
                return BadRequest("Деякі книги не знайдені");

            genre.Name = dto.Name.Trim();

            // Замінюємо зв'язки жанру з книгами.
            genre.Books.Clear();

            foreach (var book in books)
            {
                genre.Books.Add(book);
            }

            await _context.SaveChangesAsync();

            return Ok(ToDto(genre));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
                return NotFound("Жанр не знайдено");

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static GenreDto ToDto(Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                BookIds = genre.Books
                    .Select(b => b.Id)
                    .ToList()
            };
        }
    }
}
