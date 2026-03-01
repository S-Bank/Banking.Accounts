using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Models.Outbox;

/// <summary>
/// Модель сообщения в очереди исходящих событий.
/// </summary>
public sealed class Outbox
{
    /// <summary>
    /// Уникальный идентификатор записи.
    /// </summary>
    public OutboxId Id { get; private set; }

    /// <summary>
    /// Идентификатор счета.
    /// </summary>
    public AccountId AccountId { get; private set; }

    /// <summary>
    /// Тип события.
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// Содержание события.
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// Время возникновения события.
    /// </summary>
    public DateTimeOffset OccurredOn { get; private set; }

    /// <summary>
    /// Время обработки события.
    /// </summary>
    public DateTimeOffset? ProcessedOn { get; private set; }

    /// <summary>
    /// Сообщение об ошибке при отправке.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Количество неудачных попыток отправки.
    /// </summary>
    public int ErrorCount { get; private set; }

    /// <summary>
    /// Инициализирует новый экземпляр сообщения Outbox.
    /// </summary>
    /// <param name="id">
    /// Идентификатор.
    /// </param>
    /// <param name="accountId">
    /// Идентификатор счета.
    /// </param>
    /// <param name="type">
    /// Тип события.
    /// </param>
    /// <param name="content">
    /// Данные события.
    /// </param>
    /// <param name="occurredOn">
    /// Время возникновения.
    /// </param>
    /// <param name="processedOn"
    /// >Время обработки.
    /// </param>
    /// <param name="error">
    /// Ошибка.
    /// </param>
    /// <param name="errorCount">
    /// Счетчик ошибок.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные параметры равны null.
    /// </exception>
    public Outbox(
        OutboxId id,
        AccountId accountId,
        string type,
        string content,
        DateTimeOffset occurredOn,
        DateTimeOffset? processedOn,
        string? error,
        int errorCount)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(accountId);

        if (string.IsNullOrEmpty(type))
        {
            throw new ArgumentNullException(nameof(type));
        }
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentNullException(nameof(content));
        }

        Id = id;
        AccountId = accountId;
        Type = type;
        Content = content;
        OccurredOn = occurredOn;
        ProcessedOn = processedOn;
        Error = error;
        ErrorCount = errorCount;
    }

    /// <summary>
    /// Отмечает сообщение как успешно обработанное.
    /// </summary>
    public void MarkAsProcessed() => ProcessedOn = DateTimeOffset.UtcNow;

    /// <summary>
    /// Фиксирует ошибку отправки сообщения.
    /// </summary>
    /// <param name="errorMessage">
    /// Сообщение об ошибке.
    /// </param>
    public void Fail(string errorMessage)
    {
        Error = errorMessage;
        ErrorCount++;
    }
}
