using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL;
using PV521_BooksShop.DAL.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PV521_BooksShop.BLL.Services
{
    public class BotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly AppDbContext _context;
        private readonly Dictionary<string, Func<Message, TelegramChat, CancellationToken, Task>> _commands;

        public BotService(ITelegramBotClient botClient, AppDbContext context)
        {
            _botClient = botClient;
            _context = context;
            _commands = new Dictionary<string, Func<Message, TelegramChat, CancellationToken, Task>>()
            {
                    {"start", StartCommand },
                    {"weather", WeatherCommand },
                    {"subscribe", SubscribeCommand },
            };
        }

        public async Task UpdateHandlerAsync(Update update, CancellationToken ct = default)
        {
            var message = update.Message!;

            var telegramChat = await SaveChatAsync(message, ct);

            if (message.Text != null && telegramChat != null)
            {
                bool res = _commands.TryGetValue(message.Text.Trim().Replace("/", ""), out var command);

                if (!res || command == null)
                {
                    await _botClient.SendMessage(message.Chat.Id, "Невідома команда", cancellationToken: ct);
                    return;
                }

                await command(message, telegramChat, ct);
            }
        }

        public async Task<TelegramChat?> SaveChatAsync(Message message, CancellationToken ct = default)
        {
            try
            {
                var chatId = message.Chat.Id;
                var chat = await _context.TelegramChats.FirstOrDefaultAsync(e => e.ChatId == chatId, ct);
                if (chat == null)
                {
                    chat = new TelegramChat
                    {
                        ChatId = chatId,
                        UserName = message.From?.Username,
                        FirstName = message.From?.FirstName,
                        LastName = message.From?.LastName,
                        Title = message.Chat.Title
                    };

                    await _context.TelegramChats.AddAsync(chat, ct);
                    await _context.SaveChangesAsync(ct);
                }

                return chat;
            }
            catch (Exception ex)
            {
                await _botClient.SendMessage(message.Chat.Id, ex.Message, cancellationToken: ct);
                return null;
            }
        }

        public async Task StartCommand(Message message, TelegramChat telegramChat, CancellationToken ct = default)
        {
            await _botClient.SendMessage(
                message.Chat.Id, 
                "Привіт, обери команду.",
                replyMarkup: StartKeyboard(telegramChat.IsSubscribe),
                cancellationToken: ct);
        }

        public async Task WeatherCommand(Message message, TelegramChat telegramChat, CancellationToken ct = default)
        {
            await _botClient.SendMessage(message.Chat.Id, "Тут буде погода", cancellationToken: ct);
        }

        public async Task SubscribeCommand(Message message, TelegramChat telegramChat, CancellationToken ct = default)
        {
            telegramChat.IsSubscribe = !telegramChat.IsSubscribe;
            await _context.SaveChangesAsync(ct);

            string subscribeMessage = telegramChat.IsSubscribe ? "підписалися" : "відписалися";
            await _botClient.SendMessage(
                message.Chat.Id, 
                $"Ви успішно {subscribeMessage}",
                replyMarkup: StartKeyboard(telegramChat.IsSubscribe),
                cancellationToken: ct);
        }

        private ReplyKeyboardMarkup StartKeyboard(bool isSubscribe)
        {
            var keyboard = new ReplyKeyboardMarkup(
            [
                ["start", "weather", "video" ],
                ["audio", "about", isSubscribe ? "unsubscribe" : "subscribe"]
            ]);

            return keyboard;
        }
    }
}
