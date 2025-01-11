using System.Text.Json;
using Frontend.App.Config;
using Frontend.App.Kafka;
using Frontend.Entities.Account.Lib.Exceptions;
using Frontend.Entities.Account.Model;
using Frontend.Entities.ActionLog;
using Frontend.Features.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Frontend.Features.Services;

internal sealed class AccountService : IAccountService
{
    private readonly RegisterProducer _registerProducer;
    private readonly string _authorizationHost;
    private readonly ILogService _logService;

    public AccountService(
        ILogger<RegisterProducer> registerProducerLogger,
        IOptions<KafkaSettings> kafkaSettings,
        ILogService logService)
    {
        _logService = logService;
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
        await _logService.SendLog(new AddLogRequest()
        {
            Description = "Заявка на регистрацию отправлена",
            Email = registerModel.Email,
            Error = null,
            MicroserviceId = 1,
            MicroserviceName = "Authorization",
            Status = "Processing",
            Username = registerModel.Username,
            Moment = DateTimeOffset.Now
        });
    }
}