using Microsoft.Extensions.Options;
using PV521_BooksShop.Settings;
using Telegram.Bot;

namespace PV521_BooksShop.Configure
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly BotSettings _settings;

        public ConfigureWebhook(IServiceProvider services, IOptions<BotSettings> options)
        {
            _services = services;
            _settings = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            await botClient.SetWebhook(
                url: _settings.Host + "/api/bot",
                secretToken: _settings.SecretToken,
                cancellationToken: cancellationToken
                );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
