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

    public async Task Consume(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        while (_consumer.Consume(stoppingToken) is { } result)
        {
            _logger.LogInformation("Register request: {result}", result.Message.Value);
            try
            {
                var registerResult = await TryRegister(result, stoppingToken);
                
                if (registerResult)
                {
                    _logger.LogInformation("Register request successfully processed!");
                }
                else
                {
                    _logger.LogInformation("Register request failed!");
                }
            }
            catch (JsonException e)
            {
                _logger.LogError("Register request throw exception: {message}!", e.Message);
            }
            catch (InvalidRegisterException e)
            {
                _logger.LogError("Register request throw exception: {message}!", e.Message);
            }
            catch (ValidationException e)
            {
                _logger.LogWarning("{exception}", e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register request throw unhandled exception.");
            }

            _consumer.Commit(result);
        }
    }
    
    private async Task<bool> TryRegister(ConsumeResult<Ignore,string> result, CancellationToken ct)
    {
        RegisterUserModel? userModel = JsonSerializer.Deserialize<RegisterUserModel>(result.Message.Value);
        if (userModel == null)
        {
            throw new InvalidRegisterException("RegisterUserModel is null");
        }
        return await _accountService.RegisterUser(userModel, ct);
    } 

    public void Dispose()
    {
        _consumer.Close();
    }
}
