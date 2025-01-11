using Frontend.Entities.ActionLog;
using Frontend.Features.Interfaces;

namespace Frontend.Features;

public class LogService : ILogService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public LogService()
    {
        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        };

        _httpClient = new HttpClient(httpClientHandler);
        
        var host = Environment.GetEnvironmentVariable("ACTION_LOG_HOST") ?? "localhost";
        _baseUrl = $"http://{host}:8086/api/logs";
    }

    public async Task SendLog(AddLogRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/add", request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to send log: {response.StatusCode}, {errorContent}");
        }
    }

    public async Task<IReadOnlyCollection<LogModel>> GetLogs(GetLogsRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var queryParams = new Dictionary<string, string?>
        {
            ["Page"] = request.Page.ToString(),
            ["Size"] = request.Size.ToString(),
            ["Username"] = request.Username,
            ["Email"] = request.Email,
            ["MicroserviceId"] = request.MicroserviceId?.ToString(),
            ["MicroserviceName"] = request.MicroserviceName,
            ["Description"] = request.Description,
            ["Status"] = request.Status,
            ["HasError"] = request.HasError?.ToString(),
            ["From"] = request.From?.ToString("o"),
            ["To"] = request.To?.ToString("o")
        };

        var queryString = string.Join("&", queryParams
            .Where(kv => !string.IsNullOrEmpty(kv.Value))
            .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value!)}"));

        var response = await _httpClient.GetAsync($"{_baseUrl}/getlist?{queryString}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get logs: {response.StatusCode}, {errorContent}");
        }

        var result = await response.Content.ReadFromJsonAsync<GetLogsResponse>();
        if (result == null)
            throw new Exception("Failed to deserialize logs response.");

        return result.Logs.ToList();
    }
}