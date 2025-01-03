using Frontend.Entities.Revolver.Model;
using Frontend.Features.Interfaces;

namespace Frontend.Features;

public class RevolverService : IRevolverService
{
    public async Task<bool> Shoot()
    {
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
        using var httpClient = new HttpClient(httpClientHandler);
        
        List<string> baseBullet = ["Authorization", "Revolver"];
        ShootRequest request = new ShootRequest() { Bullets = baseBullet };
        JsonContent content = JsonContent.Create(request);
        
        string revolverHost = Environment.GetEnvironmentVariable("REVOLVER_HOST") ?? "localhost";
        var response = await httpClient.PostAsync("http://" + revolverHost + ":8084/api/gun/shoot", content);
        return response.IsSuccessStatusCode;
    }
}