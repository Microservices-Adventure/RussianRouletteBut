using Frontend.Entities.Revolver.Model;
using Frontend.Features.Interfaces;

namespace Frontend.Features;

public class RevolverService : IRevolverService
{
    public async Task<KilledServiceInfo> Shoot()
    {
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
        using var httpClient = new HttpClient(httpClientHandler);
        
        List<string> baseBullet = ["Authorization", "Revolver", "ActionLog"];
        ShootRequest request = new ShootRequest() { Bullets = baseBullet };
        JsonContent content = JsonContent.Create(request);
        
        string revolverHost = Environment.GetEnvironmentVariable("REVOLVER_HOST") ?? "localhost";
        var response = await httpClient.PostAsync("http://" + revolverHost + ":8084/api/gun/shoot", content);
        if (response.IsSuccessStatusCode)
        {
            KilledServiceInfo? info = await response.Content.ReadFromJsonAsync<KilledServiceInfo>();
            if (info == null)
            {
                throw new NotImplementedException();
            }

            return info;
        }
        
        return null;
    }
}