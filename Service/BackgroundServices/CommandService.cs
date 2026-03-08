using Banking.Accounts.Abstractions.Infrastructure.Transport.Processor;

namespace Banking.Accounts.Service.BackgroundServices;

public sealed class CommandService : BackgroundService
{
    public CommandService(
        ILogger<CommandService> logger,
        ICommandProcessor processor)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(processor);

        _processor = processor;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Command service запущен");

        try
        {
            await _processor.ProcessAsync(stoppingToken);
        }
        catch (OperationCanceledException ex)
        {
        }
        finally
        {

        }
    }

    private readonly ICommandProcessor _processor;
    private readonly ILogger<CommandService> _logger;
}
