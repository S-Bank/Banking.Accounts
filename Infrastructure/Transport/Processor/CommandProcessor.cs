using System.Text;
using Banking.Accounts.Abstractions.Infrastructure.Transport;
using Banking.Accounts.Abstractions.Infrastructure.Transport.Processor;
using Banking.Accounts.Models.Configurations;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Banking.Accounts.Infrastructure.Transport.Processor;

public sealed class CommandProcessor : ICommandProcessor
{
    public CommandProcessor(
        IServiceProvider serviceProvider,
        ILogger<CommandProcessor> logger,
        ICommandFactory commandFactory,
        IOptions<KafkaOptions> options)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(commandFactory);

        _serviceProvider = serviceProvider;
        _logger = logger;
        _commandFactory = commandFactory;
        _options = options.Value;
    }

    public async Task ProcessAsync(CancellationToken token)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = _options.AccountCommands.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(_options.AccountCommands.Topic);

        while (true)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                var result = consumer.Consume(token);
                if (result is null)
                {
                    continue;
                }

                await ProcessMessage(result, token);

                consumer.Commit(result);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Ошибка обработки сообщения. Offset не закоммичен.");
            }
        }
    }

    private async Task ProcessMessage(ConsumeResult<string, string> result, CancellationToken ct)
    {
        var headerBytes = result.Message.Headers.GetLastBytes("Message-Type");
        var messageType = Encoding.UTF8.GetString(headerBytes);

        if (messageType is null)
        {
            throw new ArgumentNullException(nameof(messageType));
        }

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = _commandFactory.CreateCommand(
            result.Message.Key,
            messageType,
            result.Message.Value);

        await mediator.Send(command, ct);
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandFactory _commandFactory;
    private readonly ILogger<CommandProcessor> _logger;
    private readonly KafkaOptions _options;

}
