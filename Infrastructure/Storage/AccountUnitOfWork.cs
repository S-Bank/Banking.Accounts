using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;
using Banking.Accounts.Infrastructure.Storage.Context;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Banking.Accounts.Infrastructure.Storage;

/// <summary>
/// Реализация единицы работы для управления репозиториями счетов.
/// </summary>
public sealed class AccountUnitOfWork : UnitOfWork, IAccountUnitOfWork
{
    /// <inheritdoc />
    public IAccountRepository Accounts { get; }

    /// <inheritdoc />
    public ITransactionRepository Transactions { get; }

    /// <inheritdoc />
    public IOutboxRepository Outbox { get; }


    /// <summary>
    /// Инициализирует новый экземпляр единицы работы.
    /// </summary>
    /// <param name="dataSourceProvider">
    /// Провайдер источника данных.
    /// </param>
    /// <param name="context">
    /// Контекст базы данных.
    /// </param>
    /// <param name="accountRepository">
    /// Репозиторий для работы со счетами.
    /// </param>
    /// <param name="transactionRepository">
    /// Репозиторий для работы с операциями.
    /// </param>
    /// <param name="outboxRepository">
    /// Репозиторий для работы с отправкой сообщений.
    /// </param>
    /// <param name="logger">
    /// Экземпляр логгера.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
    public AccountUnitOfWork(
        IDataSourceProvider dataSourceProvider,
        AccountContext context,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IOutboxRepository outboxRepository,
        ILogger<AccountUnitOfWork> logger)
        : base(
            dataSourceProvider,
            context,
            IsolationLevel.ReadCommitted,
            logger)
    {
        ArgumentNullException.ThrowIfNull(accountRepository);
        ArgumentNullException.ThrowIfNull(transactionRepository);
        ArgumentNullException.ThrowIfNull(outboxRepository);

        Accounts = accountRepository;
        Transactions = transactionRepository;
        Outbox = outboxRepository;
    }
}