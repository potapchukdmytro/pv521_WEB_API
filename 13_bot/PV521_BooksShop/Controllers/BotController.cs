using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Services;
using Telegram.Bot.Types;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        private readonly BotService _botService;

        public BotController(BotService botService)
        {
            _botService = botService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync([FromBody] Update update, CancellationToken ct = default)
        {
            if(update.Message == null)
            {
                return Ok();
            }

            await _botService.UpdateHandlerAsync(update, ct);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Bot started");
        }
    }
}
