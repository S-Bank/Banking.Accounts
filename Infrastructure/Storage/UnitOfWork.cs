using Banking.Accounts.Infrastructure.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Banking.Accounts.Infrastructure.Storage;

/// <summary>
/// Реализация Unit of Work для <see cref="AccountContext"/>.
/// </summary>
public class UnitOfWork : UnitOfWorkBase<AccountContext>
{
    /// <summary>
    /// Инициализирует Unit of Work с настройкой источника данных.
    /// </summary>
    /// <param name="dataSourceProvider">
    /// Провайдер источника данных.
    /// </param>
    /// <param name="context">
    /// Контекст базы данных.
    /// </param>
    /// <param name="isolationLevel">
    /// Уровень изоляции транзакции.
    /// </param>
    /// <param name="logger">
    /// Логгер.
    /// </param>
    protected UnitOfWork(
        IDataSourceProvider dataSourceProvider,
        AccountContext context,
        IsolationLevel isolationLevel,
        ILogger logger)
        : base(
            SetDefaultDataSource(dataSourceProvider, context),
            isolationLevel,
            logger)
    {
    }

    private static TContext SetDefaultDataSource<TContext>(
        IDataSourceProvider dataSourceProvider,
        TContext context)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(dataSourceProvider);
        ArgumentNullException.ThrowIfNull(context);

        context.Database.SetDbDataSource(dataSourceProvider.GetDataSource());

        return context;
    }
}
