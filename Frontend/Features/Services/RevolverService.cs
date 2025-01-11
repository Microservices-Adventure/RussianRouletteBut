using Frontend.Entities.ActionLog;
using Frontend.Entities.Revolver.Model;
using Frontend.Features.Services.Interfaces;

namespace Frontend.Features.Services;

public class RevolverService : IRevolverService
{
    private readonly ILogService _logService;

    public RevolverService(ILogService logService)
    {
        _logService = logService;
    }
    
    public async Task<KilledServiceInfo> Shoot(ShootMan shootMan)
    {
        await _logService.SendLog(new AddLogRequest()
        {
            Description = "Произошёл, так называемый, выстрел...",
            Email = shootMan.Email,
            Error = null,
            MicroserviceId = 2,
            MicroserviceName = "Revolver",
            Status = "Processing",
            Username = shootMan.Username,
            Moment = DateTimeOffset.Now
        });
        
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
        using var httpClient = new HttpClient(httpClientHandler);
        
        List<string> baseBullet = ["Authorization", "Revolver", "ActionLog", "Profile"];
        ShootRequest request = new ShootRequest() { Bullets = baseBullet };
        JsonContent content = JsonContent.Create(request);
        
        string revolverHost = Environment.GetEnvironmentVariable("REVOLVER_HOST") ?? "localhost";
        var response = await httpClient.PostAsync("http://" + revolverHost + ":8084/api/gun/shoot", content);
        if (response.IsSuccessStatusCode)
        {
            KilledServiceInfo info = (await response.Content.ReadFromJsonAsync<KilledServiceInfo>())!;

            return info;
        }
        
        await _logService.SendLog(new AddLogRequest()
        {
            Description = "Какому-то сервису повезло выжить после не удачного выстрела",
            Email = shootMan.Email,
            Error = null,
            MicroserviceId = 2,
            MicroserviceName = "Revolver",
            Status = "Fail",
            Username = shootMan.Username,
            Moment = DateTimeOffset.Now
        });
        
        
        return null;
    }
}