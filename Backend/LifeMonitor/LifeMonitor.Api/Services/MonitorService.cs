using LifeMonitor.Api.Models;
using System.ComponentModel;
using System.Text.Json;

namespace LifeMonitor.Api.Services
{
    public class MonitorService : IMonitorService
    {
        public async Task<LifeServiceModel> GetLife(string serviceHost, string port)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
            using var httpClient = new HttpClient(httpClientHandler);
            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync("http://" + serviceHost + ":" + port + "/api/hearth/isLive");
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    bool isLive = await response.Content.ReadFromJsonAsync<bool>();
                    LifeServiceModel lifeService = new LifeServiceModel()
                    {
                        IsLife = isLive,
                        ServiceName = serviceHost
                    };

                    if (!isLive)
                    {
                        return lifeService;
                    }

                    using HttpResponseMessage responseKill = await httpClient.GetAsync("http://" + serviceHost + ":" + port + "/api/hearth/killAvailable");

                    Console.WriteLine(responseKill);
                    if (responseKill.IsSuccessStatusCode)
                    {
                        CooldownModel jsonResponseCooldown = (await responseKill.Content.ReadFromJsonAsync<CooldownModel>())!;
                        return lifeService with { Cooldown = jsonResponseCooldown };

                    }

                    return new LifeServiceModel()
                    {
                        IsLife = false,
                        ServiceName = serviceHost
                    };
                }

                return new LifeServiceModel()
                {
                    IsLife = false,
                    ServiceName = serviceHost
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new LifeServiceModel()
                {
                    IsLife = false,
                    ServiceName = serviceHost
                };
            }
        }


    }

    

}
