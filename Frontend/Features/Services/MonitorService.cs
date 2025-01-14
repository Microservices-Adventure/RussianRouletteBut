using System.Text.Json;
using Frontend.Entities.LifeMonitor.Model;
using Frontend.Features.Services.Interfaces;

namespace Frontend.Features.Services;

public class MonitorService : IMonitorService
{
    private readonly HttpClient _httpClient;

    public MonitorService()
    {
        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        };

        _httpClient = new HttpClient(httpClientHandler);
    }

    public async Task<IReadOnlyCollection<LifeServiceModel>> GetLifes()
    {
        var url = "http://lifemonitor:8090/api/life/getlifes";

        try
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadFromJsonAsync<List<LifeServiceModel>>();

            return jsonResponse;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Ошибка при запросе к API: " + ex.Message);
        }
        catch (JsonException ex)
        {
            Console.WriteLine("Ошибка при десериализации ответа: " + ex.Message);
        }

        return [];
    }
}