using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Infrastructure.Storage.Models;

/// <summary>
/// Счет.
/// </summary>
public sealed class Account
{
    /// <summary>
    /// Идентификатор счета.
    /// </summary>
    public required AccountId Id { get; init; }

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    public required CustomerId CustomerId { get; init; }

    /// <summary>
    /// Баланс счета.
    /// </summary>
    public required Balance Balance { get; set; }

    /// <summary>
    /// Валюта.
    /// </summary>
    public required Currency Currency { get; init; }

    /// <summary>
    /// Статус счета.
    /// </summary>
    public required Status Status { get; set; }

    /// <summary>
    /// Версия для оптимистичной блокировки.
    /// </summary>
    public int Version { get; set; }
}