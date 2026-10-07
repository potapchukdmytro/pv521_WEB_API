using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Dtos.Parser;
using PV521_BooksShop.Settings;
using Telegram.Bot.Types;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly YoutubeClient _youtube = new();

        private async Task DownloadAudioAsync(string youtubeUrl, CancellationToken ct = default)
        {
            var manifest = await _youtube.Videos.Streams.GetManifestAsync(youtubeUrl, ct);

            var audioStream = manifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            if (audioStream is null)
            {
                throw new Exception("Не вдалося знайти аудіодоріжку.");
            }

            var memoryStream = new MemoryStream();
            await _youtube.Videos.Streams.CopyToAsync(audioStream, memoryStream, cancellationToken: ct);

            var file = InputFile.FromStream(memoryStream, "test");

            //var root = Directory.GetCurrentDirectory();
            //var storage = Path.Combine(root, PathSettings.Storage);
            //var filePath = Path.Combine(storage, $"{Guid.NewGuid()}.{audioStream.Container.Name}");

            //await _youtube.Videos.Streams.DownloadAsync(audioStream, filePath, cancellationToken: ct);
        }

        [HttpGet]
        public async Task<IActionResult> Test(CancellationToken ct = default)
        {
            await DownloadAudioAsync("https://www.youtube.com/watch?v=9bFHsd3o1w0&list=RD9bFHsd3o1w0&start_radio=1", ct);

            return Ok();
        }
    }
}
