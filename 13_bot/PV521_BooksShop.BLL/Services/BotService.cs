using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.BLL.Dtos.Parser;
using PV521_BooksShop.DAL;
using PV521_BooksShop.DAL.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace PV521_BooksShop.BLL.Services
{
    public enum Command
    {
        None,
        YoutubeAudio
    }

    public class BotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly AppDbContext _context;
        private readonly Dictionary<List<string>, Func<Message, TelegramChat, CancellationToken, Task>> _commands;

        public BotService(ITelegramBotClient botClient, AppDbContext context)
        {
            _botClient = botClient;
            _context = context;
            _commands = new Dictionary<List<string>, Func<Message, TelegramChat, CancellationToken, Task>>()
            {
                    {["start", "головна"], StartCommand },
                    {["weather", "погода"], WeatherCommand },
                    {["subscribe", "unsubscribe", "підписатися", "відписатися"], SubscribeCommand },
                    {["графіки відключень"], DisconnectionsCommand },
                    {["youtube аудіо"], YoutubeAudioCommand }
            };
        }

        public async Task UpdateHandlerAsync(Update update, CancellationToken ct = default)
        {
            var message = update.Message!;

            var telegramChat = await SaveChatAsync(message, ct);

            if (message.Text != null && telegramChat != null)
            {
                var lastCommand = Enum.Parse<Command>(telegramChat.LastCommand);

                if(lastCommand == Command.YoutubeAudio)
                {
                    await SendYoutubeAudioAsync(message, telegramChat, ct);
                    return;
                }

                var commandText = message.Text.Trim().Replace("/", "").ToLower();

                var key = _commands.Keys.FirstOrDefault(k => k.Contains(commandText));

                if (key == null || _commands[key] == null)
                {
                    await _botClient.SendMessage(message.Chat.Id, "Невідома команда", cancellationToken: ct);
                    return;
                }

                var command = _commands[key];

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

        private string GetInnerText(HtmlNode node)
        {
            var child = node.FirstChild;

            if (child == null)
            {
                return node.InnerHtml;
            }

            return GetInnerText(child);
        }

        public async Task YoutubeAudioCommand(Message message, TelegramChat telegramChat, CancellationToken ct = default)
        {
            telegramChat.LastCommand = Enum.GetName(Command.YoutubeAudio) ?? "YoutubeAudio";

            await _context.SaveChangesAsync(ct);

            await _botClient.SendMessage(
                message.Chat.Id,
                "Відправ посилання на yotube відео",
                cancellationToken: ct);
        }

        public async Task SendYoutubeAudioAsync(Message message, TelegramChat telegramChat, CancellationToken ct = default)
        {
            var url = message.Text;

            if(string.IsNullOrEmpty(url))
            {
                await _botClient.SendMessage(
                    message.Chat.Id,
                    "Посилання не може бути порожнім. Очікую посилання",
                    replyMarkup: StartKeyboard(telegramChat.IsSubscribe),
                    cancellationToken: ct);
                return;
            }

            try
            {
                using YoutubeClient youtube = new();

                var manifest = await youtube.Videos.Streams.GetManifestAsync(url, ct);
                var video = await youtube.Videos.GetAsync(url, ct);

                var audioStream = manifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                if (audioStream is null)
                {
                    await _botClient.SendMessage(
                        message.Chat.Id,
                        "Не вдалося знайти аудіодоріжку",
                        replyMarkup: StartKeyboard(telegramChat.IsSubscribe),
                        cancellationToken: ct);

                    telegramChat.LastCommand = Enum.GetName(Command.None) ?? "None";
                    await _context.SaveChangesAsync(ct);
                    return;
                }

                // Stream size
                if(audioStream.Size.MegaBytes >= 50.0)
                {
                    await _botClient.SendMessage(
                        message.Chat.Id,
                        $"Перевищено ліміт у 50Мб. Ваш файл займає {audioStream.Size.MegaBytes:0.##}Мб",
                        replyMarkup: StartKeyboard(telegramChat.IsSubscribe),
                        cancellationToken: ct);

                    telegramChat.LastCommand = Enum.GetName(Command.None) ?? "None";
                    await _context.SaveChangesAsync(ct);
                    return;
                }

                using var memoryStream = new MemoryStream();
                await youtube.Videos.Streams.CopyToAsync(audioStream, memoryStream, cancellationToken: ct);

                memoryStream.Position = 0;
                var audioFile = InputFile.FromStream(memoryStream, fileName: $"{video.Title}.{audioStream.Container.Name}");

                telegramChat.LastCommand = Enum.GetName(Command.None) ?? "None";
                await _context.SaveChangesAsync(ct);

                await _botClient.SendAudio(
                        message.Chat.Id,
                        audioFile,
                        replyMarkup: StartKeyboard(telegramChat.IsSubscribe),
                        cancellationToken: ct);
            }
            catch (Exception ex)
            {
                await _botClient.SendMessage(
                    message.Chat.Id,
                    "Не вдалося отримати аудіо за цим посиланням. " + ex.Message,
                    replyMarkup: StartKeyboard(telegramChat.IsSubscribe),
                    cancellationToken: ct);

                telegramChat.LastCommand = Enum.GetName(Command.None) ?? "None";
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task DisconnectionsCommand(Message message, TelegramChat telegramChat, CancellationToken ct = default)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync("https://www.roe.vsei.ua/disconnections", ct);

            var queueRow = doc.DocumentNode.SelectSingleNode("//*[@id=\"fetched-data-container\"]/table/tbody/tr[4]");
            var todayRow = doc.DocumentNode.SelectSingleNode("//*[@id=\"fetched-data-container\"]/table/tbody/tr[5]");

            if (queueRow == null || todayRow == null)
            {
                return;
            }

            List<DisconnectDto> disconnections = [];

            for (int i = 0; i < queueRow.ChildNodes.Count; i++)
            {
                var disconnect = new DisconnectDto
                {
                    Title = queueRow.ChildNodes[i].InnerHtml,
                    Today = GetInnerText(todayRow.ChildNodes[i])
                };

                disconnections.Add(disconnect);
            }

            string text = disconnections[0].Today + "\n";
            text += string.Join('\n', disconnections.Skip(1).Select(d => d.ToString()));

            await _botClient.SendMessage(
                message.Chat.Id,
                text,
                replyMarkup: StartKeyboard(telegramChat.IsSubscribe),
                cancellationToken: ct);
        }

        private ReplyKeyboardMarkup StartKeyboard(bool isSubscribe)
        {
            var keyboard = new ReplyKeyboardMarkup(
            [
                ["Головна", "Погода", "video" ],
                ["Youtube аудіо", "Графіки відключень", isSubscribe ? "Відписатися" : "Підписатися"]
            ]);

            return keyboard;
        }
    }
}
