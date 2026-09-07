using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookRepostiory _bookRepostiory;

        public BooksController(BookRepostiory bookRepostiory)
        {
            _bookRepostiory = bookRepostiory;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _bookRepostiory.Books;
            return Ok(books);
        }

        [HttpPost]
        public IActionResult Create()
        {
            var books = new Book[]
            {
                new Book{ Title = "test"},
                new Book{ Title = "tes2"},
            };

            return Ok("Книгу додано");
        }

        [HttpPut]
        public IActionResult Update()
        {
            return Ok("Книгу оновлено");
        }
    }
}
