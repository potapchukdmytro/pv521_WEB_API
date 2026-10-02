using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Dtos.User;
using PV521_BooksShop.BLL.Services;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UserService _userService;

        public BotController(ITelegramBotClient botClient, UserService userService)
        {
            _botClient = botClient;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync([FromBody] Update update, CancellationToken ct = default)
        {
            if(update.Message == null)
            {
                return Ok();
            }

            long chatId = update.Message.Chat.Id;
            string messageText = update.Message.Text?.Trim() ?? string.Empty;

            if(messageText.ToLower().Contains("start"))
            {
                await _botClient.SendMessage(chatId: chatId, text: "Привіт, обери команду.", cancellationToken: ct);
            }
            else if (messageText.ToLower().Contains("dice"))
            {
                await _botClient.SendDice(chatId: chatId, cancellationToken: ct);
            }
            else if (messageText.ToLower().Contains("location"))
            {
                await _botClient.SendLocation(chatId: chatId, longitude: 31.1342, latitude: 29.9792, cancellationToken: ct);
            }
            else if (messageText.ToLower().Contains("html"))
            {
                await _botClient.SendHtml(chatId: chatId, html: "<h1 style=\"color=red;\">HTML</h1>");
            }
            else if (messageText.ToLower().Contains("users"))
            {
                var response = await _userService.GetAllAsync(ct);
                if(response.IsSuccess)
                {
                    var users = response.Payload as List<UserDto>;
                    if(users != null)
                    {
                        string text = string.Join("\n============\n", users.Select(u => u.ToString()));
                        await _botClient.SendMessage(chatId: chatId, text: text, cancellationToken: ct);
                    }
                }
                else
                {
                    await _botClient.SendMessage(chatId: chatId, text: response.Message, cancellationToken: ct);
                }
            }
            else
            {
                await _botClient.SendMessage(chatId: chatId, text: "Невідома команда", cancellationToken: ct);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Bot started");
        }
    }
}
