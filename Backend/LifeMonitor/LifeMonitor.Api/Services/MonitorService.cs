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
            HttpResponseMessage response = await httpClient.GetAsync("http://" + serviceHost + ":" + port + "/api/hearth/isLive");
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                bool isLive = JsonSerializer.Deserialize<bool>(jsonResponse);
                LifeServiceModel lifeService = new LifeServiceModel()
                {
                    IsLife = isLive,
                    ServiceName = serviceHost
                };

                if (!isLive)
                {
                    return lifeService;
                }

                HttpResponseMessage responseKill = await httpClient.GetAsync("http://" + serviceHost + ":" + port + "/api/hearth/killAvailable");

                if (responseKill.IsSuccessStatusCode)
                {
                    CooldownModel jsonResponseCooldown = (await response.Content.ReadFromJsonAsync<CooldownModel>())!;
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


    }

    

}
