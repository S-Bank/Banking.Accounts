using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Outbox;

namespace Banking.Accounts.Infrastructure.Storage.Models;

// <summary>
/// Модель данных для хранения исходящих событий.
/// </summary>
public sealed class Outbox
{
    /// <summary>
    /// Идентификатор записи.
    /// </summary>
    public required OutboxId Id { get; init; }

    /// <summary>
    /// Идентификатор счета.
    /// </summary>
    public required AccountId AccountId { get; init; }

    /// <summary>
    /// Тип события.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Сериализованные данные события.
    /// </summary>
    public required string Content { get; init; }

    /// <summary>
    /// Время создания записи.
    /// </summary>
    public required DateTimeOffset OccurredOn { get; init; }

    /// <summary>
    /// Время успешной обработки.
    /// </summary>
    public required DateTimeOffset? ProcessedOn { get; set; }

    /// <summary>
    /// Сообщение об ошибке при отправке.
    /// </summary>
    public required string? Error { get; set; }

    /// <summary>
    /// Счетчик неудачных попыток отправки.
    /// </summary>
    public required int ErrorCount { get; set; }
}
