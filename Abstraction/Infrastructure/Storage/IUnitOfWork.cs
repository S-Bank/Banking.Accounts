namespace Banking.Accounts.Abstractions.Infrastructure.Storage;

/// <summary>
/// Контракт единицы работы для сохранения изменений.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохраняет изменения в хранилище.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Задача, представляющая операцию сохранения.
    /// </returns>
    Task SaveAsync(CancellationToken token);
}