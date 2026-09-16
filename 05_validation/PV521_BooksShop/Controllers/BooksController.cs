using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Dtos;
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
        private readonly IValidator<CreateBookDto> _validatiorCreate;
        private readonly IValidator<UpdateBookDto> _validatiorUpdate;

        public BooksController(BookService bookService, IWebHostEnvironment env, IValidator<CreateBookDto> validatiorCreate, IValidator<UpdateBookDto> validatiorUpdate)
        {
            _bookService = bookService;
            _imagesPath = Path.Combine(env.ContentRootPath, PathSettings.Books);
            _validatiorCreate = validatiorCreate;
            _validatiorUpdate = validatiorUpdate;
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
            var response = await _bookService.DeleteAsync(id, _imagesPath);
            return this.GetHttpResponse(response);
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBookDto dto)
        {
            var validation = _validatiorCreate.Validate(dto);

            if (!validation.IsValid)
            {
                return this.GetValiationErrorResponse(validation);
            }

            var response = await _bookService.CreateAsync(dto, _imagesPath);
            return this.GetHttpResponse(response);
        }

        // Update
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateBookDto dto)
        {
            var validation = _validatiorUpdate.Validate(dto);

            if (!validation.IsValid)
            {
                return this.GetValiationErrorResponse(validation);
            }

            var response = await _bookService.UpdateAsync(dto, _imagesPath);
            return this.GetHttpResponse(response);
        }
    }
}
