using Revolver.Domain.Config;
using Revolver.Domain.Models;
using Revolver.Domain.Services.Interfaces;

namespace Revolver.Domain.Services;

public sealed class RevolverService : IRevolverService
{
    private readonly Random _random;

    public RevolverService()
    {
        _random = new Random();
    }
    
    public ServiceInfo Roll(IReadOnlyList<ServiceInfo> services)
    {
        return services[_random.Next(0, services.Count)];
    }

    public async Task<bool> Kill(ServiceInfo serviceInfo)
    {
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
        using var httpClient = new HttpClient(httpClientHandler);
        using HttpResponseMessage responseMessage = await httpClient.PostAsync(serviceInfo.Host + "/api/hearth/kill", null);
        return responseMessage.IsSuccessStatusCode;
    }
}