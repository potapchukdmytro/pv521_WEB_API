using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PV521_BooksShop.Jobs
{
    public class TelegramSubscribeJob : IJob
    {
        private readonly AppDbContext _context;
        private readonly ITelegramBotClient _botClient;

        public TelegramSubscribeJob(AppDbContext context, ITelegramBotClient botClient)
        {
            _context = context;
            _botClient = botClient;
        }

        public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            var chats = await _context.TelegramChats.Where(e => e.IsSubscribe).ToListAsync(cancellationToken);

            var tasks = chats.Select(c => _botClient.SendMessage(c.ChatId, "Привіт. Я твій помічник"));

            await Task.WhenAll(tasks);
        }
    }
}
