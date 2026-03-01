using Banking.Accounts.Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Banking.Accounts.Infrastructure.Storage.Context;

/// <summary>
/// Реализация контекста репозитория для управления сущностями через DbContext.
/// </summary>
public sealed class RepositoryContext : IRepositoryContext
{
    /// </inheritdoc>
    public DbSet<Account> Accounts => _context.Accounts;

    /// </inheritdoc>
    public DbSet<Transaction> Transactions => _context.Transactions;

    /// </inheritdoc>
    public DbSet<Outbox> Outbox => _context.Outboxs;

    /// <summary>
    /// Инициализирует новый экземпляр контекста репозитория.
    /// </summary>
    /// <param name="context">
    /// Контекст базы данных счетов.
    /// </param>
    /// <param name="logger">
    /// Экземпляр логгера.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
    public RepositoryContext(AccountContext context, ILogger<RepositoryContext> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    /// </inheritdoc>
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Processing version increments before save");

        _logger.LogDebug("Save intermediate changes");

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Intermediate changes sent to database.");
    }

    private readonly AccountContext _context;
    private readonly ILogger<RepositoryContext> _logger;
}
