using Banking.Accounts.Abstractions.Infrastructure.Transport.Processor;

namespace Banking.Accounts.Service.BackgroundJobs;

/// <summary>
/// Фоновый сервис для периодической обработки необработанных сообщений
/// из таблицы Outbox и их публикации в брокер сообщений.
/// </summary>
public sealed class OutboxService : BackgroundService
{
    /// <summary>
    /// Инициализирует новый экземпляр сервиса.
    /// </summary>
    /// <param name="serviceProvider">
    /// Провайдер сервисов для создания областей видимости.
    /// </param>
    /// <param name="logger">
    /// Логгер.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
    public OutboxService(
        IServiceProvider serviceProvider,
        ILogger<OutboxService> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox serivce запущен.");

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
        var publisher = scope.ServiceProvider.GetRequiredService<ISystemEventProcessor>();

        await publisher.ProcessAsync(ct);
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxService> _logger;
}
