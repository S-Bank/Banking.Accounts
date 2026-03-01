using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Abstractions.Infrastructure.Transport.Processor;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Banking.Accounts.Infrastructure.Transport.Processor;

/// <summary>
/// Реализация обработчика событий, использующая подход polling для чтения сообщений
/// из таблицы базы данных и публикации их.
/// </summary>
public sealed class SystemEventProcessor : ISystemEventProcessor
{
    /// <summary>
    /// Инициализирует новый экземпляр обработчика событий.
    /// </summary>
    /// <param name="producer">
    /// Продюсер  для публикации сообщений.
    /// </param>
    /// <param name="unitOfWork">
    /// Единица работы для взаимодействия с БД .
    /// </param>
    /// <param name="logger">
    /// Экземпляр логгера для трассировки процесса отправки.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные зависимости равны null.
    /// </exception>
    public SystemEventProcessor(
        IProducer<string, string> producer,
        IAccountUnitOfWork unitOfWork,
        ILogger<SystemEventProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(producer);
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(logger);

        _producer = producer;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ProcessAsync(CancellationToken token)
    {
        var messages = await _unitOfWork.Outbox.GetUnprocessedAsync(BATCH_SIZE, token);

        if (!messages.Any()) return;

        foreach (var message in messages)
        {
            try
            {
                var publishMessage = new Message<string, string>
                {
                    Key = message.AccountId.Value.ToString(),
                    Value = message.Content
                };

                var result = await _producer.ProduceAsync(TOPIC, publishMessage, token);
                _logger.LogInformation("Сообщение {EventType} отправлено в Kafka (Offset: {Offset})", message.Type, result.Offset);

                message.MarkAsProcessed();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Не удалось отправить сообщение {Id}", message.Id);
                message.Fail(ex.Message);
            }

            _unitOfWork.Outbox.Update(message);
        }

        await _unitOfWork.SaveAsync(token);

    }

    private readonly IAccountUnitOfWork _unitOfWork;
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<SystemEventProcessor> _logger;


    private const string TOPIC = "banking.accounts.events";

    private const int BATCH_SIZE = 20;
}
