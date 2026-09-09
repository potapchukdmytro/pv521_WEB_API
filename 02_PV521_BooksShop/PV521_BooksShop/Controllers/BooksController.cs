using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.BLL.Dtos.Author;
using PV521_BooksShop.BLL.Dtos.Book;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.Controllers
{
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly BookRepostiory _bookRepostiory;

        public BooksController(BookRepostiory bookRepostiory)
        {
            _bookRepostiory = bookRepostiory;
        }

        // All
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDto dto)
        {
            var entities = await _bookRepostiory.Books
                .Include(b => b.Author)
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

            var dtos = entities.Select(b =>
            {
                return new BookDto
                {
                    Id = b.Id,
                    Description = b.Description,
                    Title = b.Title,
                    Pages = b.Pages,
                    Price = b.Price,
                    Rating = b.Rating,
                    Year = b.Year,
                    Author = b.Author == null ? null : new AuthorDto
                    {
                        Id = b.Author.Id,
                        Biography = b.Author.Biography,
                        BirthDate = b.Author.BirthDate,
                        Country = b.Author.Country,
                        Name = b.Author.Name,
                        Image = b.Image
                    }
                };
            });

            return Ok(dtos);
        }

        // ById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var book = await _bookRepostiory.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound($"Книга з id '{id}' не знайдена");
            }

            return Ok(book);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            bool res = await _bookRepostiory.DeleteAsync(id);

            if(!res)
            {
                return BadRequest("Помилка під час видалення книги");
            }

            return Ok("Книгу успішно видалено");
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBookDto dto)
        {
            // maping
            var enitity = new Book
            {
                Description = dto.Description,
                Title = dto.Title,
                Pages = dto.Pages,
                Price = dto.Price,
                Rating = dto.Rating,
                Year = dto.Year,
                AuthorId = dto.AuthorId
            };

            bool res = await _bookRepostiory.CreateAsync(enitity);

            if (!res)
            {
                return BadRequest("Помилка під час створення книги");
            }

            return Ok("Книгу додано");
        }

        // Update
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateBookDto dto)
        {
            // maping
            var enitity = new Book
            {
                Id = dto.Id,
                Description = dto.Description,
                Title = dto.Title,
                Pages = dto.Pages,
                Price = dto.Price,
                Rating = dto.Rating,
                Year = dto.Year,
                AuthorId = dto.AuthorId
            };

            bool res = await _bookRepostiory.UpdateAsync(enitity);

            if (!res)
            {
                return BadRequest("Помилка під час оновлення книги");
            }

            return Ok("Книгу оновлено");
        }
    }
}
