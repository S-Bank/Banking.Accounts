using Banking.Accounts.Abstractions.Infrastructure.Storage;

namespace Banking.Accounts.Service.BackgroundJobs;

public sealed class OutboxProcessor : BackgroundService
{
    public OutboxProcessor(
        IServiceProvider serviceProvider,
        ILogger<OutboxProcessor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Processor запущен.");
        

        while (true)
        {
            using var timerCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, timerCts.Token);

            try
            {
                stoppingToken.ThrowIfCancellationRequested();
                await ProcessOutboxMessagesAsync(linkedCts.Token);
            }
            catch (OperationCanceledException) when (timerCts.IsCancellationRequested && !stoppingToken.IsCancellationRequested)
            {
                _logger.LogError("Итерация Outbox прервана по таймауту (10 сек). Возможна частичная отправка.");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Outbox Processor остановлен.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке сообщений Outbox.");
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IAccountUnitOfWork>();
        var publisher = scope.ServiceProvider.GetRequiredService<ISystemEventPublisher>();

        var messages = await unitOfWork.Outbox.GetUnprocessedAsync(BATCH_SIZE, ct);

        if (!messages.Any()) return;

        foreach (var message in messages)
        {
            try
            {
                await publisher.PublishAsync(message.Type, message.Content, ct);

                message.MarkAsProcessed();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Не удалось отправить сообщение {Id}", message.Id);
                message.Fail(ex.Message);
            }

            unitOfWork.Outbox.Update(message);
        }

        await unitOfWork.SaveAsync(ct);
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessor> _logger;

    private const int BATCH_SIZE = 20;
}

public interface ISystemEventPublisher
{
    Task PublishAsync(string eventType, string content, CancellationToken ct);
}

public sealed class SystemEventPublisher : ISystemEventPublisher
{
    public async Task PublishAsync(string eventType, string content, CancellationToken ct)
    {
        await Task.Delay(200,ct);
    }
}