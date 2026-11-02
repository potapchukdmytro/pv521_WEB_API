using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using PV521_BooksShop.DAL;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.Controllers
{
    public class AuthorUploadDto
    {
        [Required]
        public string Name { get; set; } = "";

        public string? Biography { get; set; }
        public string? Country { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        public IFormFile? Photo { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorRepository _repository;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AuthorsController(
            AuthorRepository repository,
            AppDbContext context,
            IWebHostEnvironment environment)
        {
            _repository = repository;
            _context = context;
            _environment = environment;
        }

        private string ImagesDirectory =>
            Path.Combine(
                _environment.ContentRootPath,
                "wwwroot", "images", "authors");

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _repository.Authors
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Biography,
                    a.Country,
                    a.BirthDate,
                    a.Image
                })
                .ToListAsync();

            return Ok(authors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author == null)
                return NotFound("Автора не знайдено");

            return Ok(ToResponse(author));
        }

        [HttpGet("{id:int}/books")]
        public async Task<IActionResult> GetBooksByAuthor(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author == null)
                return NotFound("Автора не знайдено");

            var books = await _context.Books
                .AsNoTracking()
                .Where(b => b.AuthorId == id)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Description,
                    b.Image,
                    b.Rating,
                    b.Price,
                    b.Pages,
                    b.Year,
                    b.AuthorId
                })
                .ToListAsync();

            return Ok(books);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> Create(
            [FromForm] AuthorUploadDto dto)
        {
            string? error = ValidatePhoto(dto.Photo);

            if (error != null)
                return BadRequest(error);

            string? image = dto.Photo == null
                ? null
                : await SavePhoto(dto.Photo);

            var author = new Author
            {
                Name = dto.Name,
                Biography = dto.Biography,
                Country = dto.Country,
                BirthDate = dto.BirthDate!.Value.ToUniversalTime(),
                Image = image
            };

            try
            {
                if (!await _repository.CreateAsync(author))
                {
                    DeletePhoto(image);
                    return Problem("Не вдалося додати автора");
                }
            }
            catch
            {
                DeletePhoto(image);
                throw;
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = author.Id },
                ToResponse(author));
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] AuthorUploadDto dto)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author == null)
                return NotFound("Автора не знайдено");

            string? error = ValidatePhoto(dto.Photo);

            if (error != null)
                return BadRequest(error);

            string? oldImage = author.Image;
            string? newImage = dto.Photo == null
                ? null
                : await SavePhoto(dto.Photo);

            author.Name = dto.Name;
            author.Biography = dto.Biography;
            author.Country = dto.Country;
            author.BirthDate =
                dto.BirthDate!.Value.ToUniversalTime();

            if (newImage != null)
                author.Image = newImage;

            try
            {
                if (!await _repository.UpdateAsync(author))
                {
                    DeletePhoto(newImage);
                    return Problem("Не вдалося оновити автора");
                }
            }
            catch
            {
                DeletePhoto(newImage);
                throw;
            }

            // Старе фото видаляємо після збереження змін.
            if (newImage != null)
                DeletePhoto(oldImage);

            return Ok(ToResponse(author));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author == null)
                return NotFound("Автора не знайдено");

            string? image = author.Image;

            if (!await _repository.DeleteAsync(author))
                return Problem("Не вдалося видалити автора");

            DeletePhoto(image);

            return NoContent();
        }

        private static object ToResponse(Author author)
        {
            return new
            {
                author.Id,
                author.Name,
                author.Biography,
                author.Country,
                author.BirthDate,
                author.Image
            };
        }

        private static string? ValidatePhoto(IFormFile? photo)
        {
            if (photo == null)
                return null;

            if (photo.Length == 0)
                return "Файл порожній";

            if (photo.Length > 5 * 1024 * 1024)
                return "Максимальний розмір фото — 5 МБ";

            string extension =
                Path.GetExtension(photo.FileName)
                    .ToLowerInvariant();

            if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png")
            {
                return "Дозволені тільки JPG та PNG";
            }

            using var stream = photo.OpenReadStream();

            byte[] header = new byte[8];
            int count = 0;

            while (count < header.Length)
            {
                int read = stream.Read(
                    header, count, header.Length - count);

                if (read == 0)
                    break;

                count += read;
            }

            bool jpeg = count >= 3 &&
                header[0] == 0xFF &&
                header[1] == 0xD8 &&
                header[2] == 0xFF;

            bool png = count == 8 &&
                header[0] == 0x89 &&
                header[1] == 0x50 &&
                header[2] == 0x4E &&
                header[3] == 0x47 &&
                header[4] == 0x0D &&
                header[5] == 0x0A &&
                header[6] == 0x1A &&
                header[7] == 0x0A;

            if (extension == ".png" ? !png : !jpeg)
                return "Файл не відповідає формату зображення";

            return null;
        }

        private async Task<string> SavePhoto(IFormFile photo)
        {
            Directory.CreateDirectory(ImagesDirectory);

            string extension =
                Path.GetExtension(photo.FileName)
                    .ToLowerInvariant();

            string fileName =
                Guid.NewGuid().ToString("N") + extension;

            string path =
                Path.Combine(ImagesDirectory, fileName);

            try
            {
                await using var stream =
                    new FileStream(path, FileMode.CreateNew);

                await photo.CopyToAsync(stream);
            }
            catch
            {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                throw;
            }

            return "/images/authors/" + fileName;
        }

        private void DeletePhoto(string? image)
        {
            const string prefix = "/images/authors/";

            if (string.IsNullOrWhiteSpace(image) ||
                !image.StartsWith(
                    prefix, StringComparison.Ordinal))
            {
                return;
            }

            string fileName = image.Substring(prefix.Length);
            string name =
                Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            if (fileName.Contains('/') ||
                fileName.Contains('\\') ||
                !Guid.TryParseExact(name, "N", out _) ||
                (extension != ".jpg" &&
                 extension != ".jpeg" &&
                 extension != ".png"))
            {
                return;
            }

            string path =
                Path.Combine(ImagesDirectory, fileName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }
}
