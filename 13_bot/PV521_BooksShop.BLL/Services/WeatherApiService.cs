using Microsoft.Extensions.Options;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.Api;
using PV521_BooksShop.BLL.Settings;
using System.Net.Http.Json;

namespace PV521_BooksShop.BLL.Services
{
    public class WeatherApiService
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherApiSettings _settings;

        public WeatherApiService(HttpClient httpClient, IOptions<WeatherApiSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<ServiceResponseDto> GetWeatherAsync(string city, CancellationToken ct = default)
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_settings.ApiKey}&units={_settings.Units}";
                var dto = await _httpClient.GetFromJsonAsync<WeatherDto>(url, ct);

                if(dto == null)
                {
                    return ServiceResponseDto.Error("Не вдалося отримати дані про погоду");
                }

                return ServiceResponseDto.Success("Погоду отримано", dto);
            }
            catch (Exception ex)
            {
                return ServiceResponseDto.Error(ex.Message);
            }
        }

        public static string WindDirection(int deg)
        {
            deg = (deg % 360 + 360) % 360;

            // 16 основних скорочень українською мовою
            string[] directions =
            {
            "Пн", "Пн-Пн-Сх", "Пн-Сх", "Сх-Пн-Сх",
            "Сх", "Сх-Пд-Сх", "Пд-Сх", "Пд-Пд-Сх",
            "Пд", "Пд-Пд-Зх", "Пд-Зх", "Зх-Пд-Зх",
            "Зх", "Зх-Пн-Зх", "Пн-Зх", "Пн-Пн-Зх"
            };

            // Вираховуємо індекс масиву (додаємо 11.25 для правильного округлення секторів)
            int index = (int)((deg + 11.25) / 22.5) % 16;

            return directions[index];
        }
    }
}
