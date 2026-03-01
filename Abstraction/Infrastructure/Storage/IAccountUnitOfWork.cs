using Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;

namespace Banking.Accounts.Abstractions.Infrastructure.Storage;

/// <summary>
/// Интерфейс единицы работы для работы со счетом.
/// </summary>
public interface IAccountUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Репозиторий для управления банковскими счетами.
    /// </summary>
    IAccountRepository Accounts { get; }

    /// <summary>
    /// Репозиторий для управления транзакциями.
    /// </summary>
    ITransactionRepository Transactions { get; }

    IOutboxRepository Outbox { get; }
}
