using Banking.Accounts.Models.Transaction;

namespace Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;

/// <summary>
/// Интерфейс репозитория для управления транзакциями.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Добавляет новую транзакцию в контекст данных.
    /// </summary>
    /// <param name="model">
    /// Модель транзакции для сохранения.
    /// </param>
    void Add(Transaction model);
}
