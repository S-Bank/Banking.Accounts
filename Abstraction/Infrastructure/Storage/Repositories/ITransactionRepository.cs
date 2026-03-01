using Banking.Accounts.Models.Transaction;

namespace Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;

/// <summary>
/// Интерфейс репозитория для управления транзакциями.
/// </summary>
public interface ITransactionRepository
{

    Task<bool> ExistsAsync(ReferenceId referenceId, CancellationToken token);

    void Add(Transaction model);
}
