using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Models.Exceptions;

/// <summary>
/// Исключение, возникающее при конфликте версий (оптимистичная блокировка).
/// </summary>
public sealed class AccountConflictException : Exception
{
    /// <summary>
    /// Идентификатор счета, вызвавший конфликт (опционально).
    /// </summary>
    public string? AccountId { get; }

    /// <summary>
    /// Создает экземпляр исключения с сообщением.
    /// </summary>
    /// <param name="message">
    /// Сообщение об ошибке.
    /// </param>
    public AccountConflictException(string message)
        : base(message)
    { }

    /// <summary>
    /// Создает экземпляр исключения с сообщением и внутренним исключением.
    /// </summary>
    /// <param name="message">
    /// Сообщение об ошибке.
    /// </param>
    /// <param name="innerException">
    /// Внутреннее исключение.
    /// </param>
    public AccountConflictException(string message, Exception innerException)
        : base(message, innerException)
    { }

    /// <summary>
    /// Создает экземпляр исключения на основе идентификатора счета.
    /// </summary>
    /// <param name="accountId">
    /// Объект идентификатора счета.
    /// </param>
    /// <param name="innerException">
    /// Внутреннее исключение.
    /// </param>
    public AccountConflictException(AccountId accountId, Exception innerException)
        : base($"Конфликт обновления данных для счета {accountId.Value}. Данные были изменены другим процессо.", innerException)
    {
        AccountId = accountId.Value.ToString();
    }
}