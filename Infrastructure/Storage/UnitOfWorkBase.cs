using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Banking.Accounts.Infrastructure.Storage;

/// <summary>
/// Базовая реализация Unit of Work для <see cref="DbContext"/>.
/// Управляет транзакцией и сохранением изменений.
/// </summary>
/// <typeparam name="TContext">
/// Тип контекста базы данных.
/// </typeparam>
public class UnitOfWorkBase<TContext> : IUnitOfWork, IAsyncDisposable, IDisposable where TContext : DbContext
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    protected TContext Context { get; }

    /// <summary>
    /// Логгер.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Инициализирует Unit of Work и открывает транзакцию.
    /// </summary>
    /// <param name="context">
    /// Контекст базы данных.
    /// </param>
    /// <param name="isolationLevel">
    /// Уровень изоляции транзакции.
    /// </param>
    /// <param name="logger">Логгер.</param>
    protected UnitOfWorkBase(TContext context, IsolationLevel isolationLevel, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        Context = context;
        Logger = logger;

        _dbContextTransaction = Context.Database.BeginTransaction(isolationLevel);
        Logger.LogDebug("Transaction created");

    }

    /// <summary>
    /// Освобождает ресурсы.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Освобождает ресурсы.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DisposeCoreAsync().ConfigureAwait(continueOnCapturedContext: false);
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Сохраняет изменения и фиксирует транзакцию.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Задача, представляющая операцию сохранения.
    /// </returns>
    public async Task SaveAsync(CancellationToken token)
    {
        ThrowIfDisposed();
        token.ThrowIfCancellationRequested();

        using (Logger.BeginScope("Save change"))
        {
            try
            {
                await Context.SaveChangesAsync(token).ConfigureAwait(continueOnCapturedContext: false);
                Logger.LogDebug("Changes sent to database");
                await _dbContextTransaction.CommitAsync(token).ConfigureAwait(continueOnCapturedContext: false);
                Logger.LogDebug("Transaction committed successfully");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogWarning(ex, "Concurrency conflict detected for entities: {Entities}",
                string.Join(", ", ex.Entries.Select(e => e.Entity.GetType().Name)));

                throw new AccountConflictException(
                    "Данные были изменены другим пользователем. Пожалуйста, повторите операцию.", ex);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while saving the transaction");
                throw;
            }
        }
    }

    private void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _dbContextTransaction.Dispose();
            }

            _isDisposed = true;
            Logger.LogDebug("Instance disposed");
        }
    }

    private async ValueTask DisposeCoreAsync()
    {
        await _dbContextTransaction.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

    private readonly IDbContextTransaction _dbContextTransaction;
    private bool _isDisposed;
}