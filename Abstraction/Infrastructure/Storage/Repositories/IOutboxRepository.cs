using Banking.Accounts.Models.Outbox;

namespace Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;

/// <summary>
/// Описывает репозиторий для управления сообщениями в таблице исходящих событий.
/// </summary>
public interface IOutboxRepository
{
    /// <summary>
    /// Добавляет новое сообщение в очередь Outbox.
    /// </summary>
    /// <param name="outbox">
    /// Экземпляр сообщения для сохранения.
    /// </param>
    void Add(Outbox outbox);

    /// <summary>
    /// Получает пакет необработанных сообщений из очереди.
    /// </summary>
    /// <param name="batchSize">
    /// Максимальное количество сообщений в пакете.
    /// </param>
    /// <param name="ct">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// Список необработанных сообщений.
    /// </returns>
    Task<IReadOnlyCollection<Outbox>> GetUnprocessedAsync(int batchSize, CancellationToken ct);

    /// <summary>
    /// Обновляет состояние существующего сообщения.
    /// </summary>
    /// <param name="message">
    /// Сообщение с обновленными данными.
    /// </param>
    void Update(Outbox message);
}
