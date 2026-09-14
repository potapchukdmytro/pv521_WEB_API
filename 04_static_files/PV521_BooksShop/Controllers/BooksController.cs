using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Dtos.Book;
using PV521_BooksShop.BLL.Dtos.Pagination;
using PV521_BooksShop.BLL.Services;
using PV521_BooksShop.Extensions;
using PV521_BooksShop.Settings;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly string _imagesPath;

        public BooksController(BookService bookService, IWebHostEnvironment env)
        {
            _bookService = bookService;
            _imagesPath = Path.Combine(env.ContentRootPath, PathSettings.Books);
        }

        // All
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationRequestDto dto)
        {
            var response = await _bookService.GetAllAsync(dto);
            return this.GetHttpResponse(response);
        }

        // ById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var response = await _bookService.GetByIdAsync(id);
            return this.GetHttpResponse(response);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _bookService.DeleteAsync(id);
            return this.GetHttpResponse(response);
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBookDto dto)
        {
            var response = await _bookService.CreateAsync(dto, _imagesPath);
            return this.GetHttpResponse(response);
        }

        // Update
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateBookDto dto)
        {
            var response = await _bookService.UpdateAsync(dto);
            return this.GetHttpResponse(response);
        }
    }
}
