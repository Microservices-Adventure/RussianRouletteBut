using System.Text.Json;
using Frontend.App.Config;
using Frontend.App.Kafka;
using Frontend.Entities.Account.Lib.Exceptions;
using Frontend.Entities.Account.Model;
using Frontend.Features.Interfaces;
using Microsoft.Extensions.Options;

namespace Frontend.Features;

internal sealed class AccountService : IAccountService
{
    private readonly RegisterProducer _registerProducer;
    private readonly string _authorizationHost;

    public AccountService(
        ILogger<RegisterProducer> registerProducerLogger,
        IOptions<KafkaSettings> kafkaSettings)
    {
        _registerProducer = new RegisterProducer(
            kafkaSettings.Value.BootstrapServers,
            kafkaSettings.Value.RegistrationTopic,
            registerProducerLogger);
        
        _authorizationHost = Environment.GetEnvironmentVariable("ASPNETCORE_AuthorizationService") ?? "localhost";
    }
    
    public async Task<LoginResponse> Login(LoginModel loginModel)
    {
        JsonContent jsonContent = JsonContent.Create(loginModel);
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
        
        using var httpClient = new HttpClient(httpClientHandler);
        
        using HttpResponseMessage responseMessage = await httpClient.PostAsync("http://" + _authorizationHost + ":8082/api/account/login", jsonContent);

        if (responseMessage.IsSuccessStatusCode)
        { 
            LoginResponse? loginResponse = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();
            if (loginResponse == null)
            {
                throw new ResponseLoginException();
            }
            
            return loginResponse;
        }
        
        string body = await responseMessage.Content.ReadAsStringAsync();
        throw new RequestLoginException(body);
    }

    public async Task Register(RegisterModel registerModel)
    {
        await _registerProducer.Register(registerModel);
    }
}