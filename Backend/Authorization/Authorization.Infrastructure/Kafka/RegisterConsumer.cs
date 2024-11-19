using Authorization.Domain.Exceptions;
using Authorization.Domain.Models;
using Authorization.Domain.Services.Interfaces;
using Confluent.Kafka;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Authorization.Infrastructure.Kafka;
public class RegisterConsumer : IDisposable
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<RegisterConsumer> _logger;
    private readonly IAccountService _accountService;

    public RegisterConsumer(
        string bootstrapServers,
        string topic,
        string groupId,
        ILogger<RegisterConsumer> logger,
        IAccountService accountService)
    {
        var builder = new ConsumerBuilder<Ignore, string>(
            new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            });

        _logger = logger;
        
        _consumer = builder.Build();
        _consumer.Subscribe(topic);
        _accountService = accountService;
    }

    public async Task Consume()
    {
        ConsumeResult<Ignore, string> result = _consumer.Consume();
        _logger.LogInformation("Register request: {result}", result.Message.Value);

        try
        {
            RegisterUserModel? userModel = JsonSerializer.Deserialize<RegisterUserModel>(result.Message.Value);

            if (userModel == null)
            {
                throw new InvalidRegisterException("RegisterUserModel is null");
            }

            await _accountService.RegisterUser(userModel);
            _logger.LogInformation("Register request successfully processed!");
        }
        catch (JsonException e)
        {
            _logger.LogError("Register request throw exception: {message}", e.Message);
        }
        catch (InvalidRegisterException e)
        {
            _logger.LogError("Register request throw exception: {message}", e.Message);
        }
        catch (ValidationException e)
        {
            _logger.LogWarning("{exception}", e.Message);
        }

        _consumer.Commit(result);
    }

    public void Dispose()
    {
        _consumer.Close();
    }
}
