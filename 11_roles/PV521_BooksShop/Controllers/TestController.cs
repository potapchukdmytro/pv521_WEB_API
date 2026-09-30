using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            throw new Exception("Generate exception");

            return Ok();
        }
    }
}
