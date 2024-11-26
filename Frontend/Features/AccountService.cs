using Frontend.App.Config;
using Frontend.App.Kafka;
using Frontend.Entities.Account.Model;
using Frontend.Features.Interfaces;
using Microsoft.Extensions.Options;

namespace Frontend.Features;

public class AccountService : IAccountService
{
    private readonly RegisterProducer _registerProducer;

    public AccountService(
        ILogger<RegisterProducer> registerProducerLogger,
        IOptions<KafkaSettings> kafkaSettings)
    {
        _registerProducer = new RegisterProducer(
            kafkaSettings.Value.BootstrapServers,
            kafkaSettings.Value.RegisterTopic,
            registerProducerLogger);
    }
    
    public Task Login(LoginModel loginModel)
    {
        throw new NotImplementedException();
    }

    public async Task Register(RegisterModel registerModel)
    {
        await _registerProducer.Register(registerModel);
    }
}