using Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;
using Banking.Accounts.Infrastructure.Storage.Context;
using Microsoft.Extensions.Logging;

namespace Banking.Accounts.Infrastructure.Storage.Repositories;

/// <summary>
/// Репозиторий для создания и хранения записей о транзакциях.
/// </summary>
public sealed class TransactionRepository :
    ITransactionRepository
{
    /// <summary>
    /// Инициализирует новый экземпляр репозитория транзакций.
    /// </summary>
    /// <param name="context">
    /// Контекст репозитория для доступа к базе данных.
    /// </param>
    /// <param name="logger">
    /// Экземпляр логгера.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
    public TransactionRepository(
        IRepositoryContext context,
        ILogger<TransactionRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public void Add(Accounts.Models.Transaction.Transaction model)
    {
        var dbModel = new Models.Transaction
        {
            Id = model.Id,
            AccountId = model.AccountId,
            ReferenceId = model.ReferenceId,
            Type = model.Type,
            Amount = model.Amount,
            CreatedAt = model.CreatedAt
        };

        _context.Transactions.Add(dbModel);
    }

    private readonly IRepositoryContext _context;
    private readonly ILogger<TransactionRepository> _logger;
}
