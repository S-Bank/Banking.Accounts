using Banking.Accounts.Infrastructure.Storage.Configurations;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Transaction;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data.Common;

namespace Banking.Accounts.Infrastructure.Storage;

/// <summary>
/// Провайдер источников данных для работы с базой данных PostgreSQL.
/// </summary>
public sealed class DataSourceProvider : IDataSourceProvider
{
    /// <summary>
    /// Инициализирует новый экземпляр провайдера источников данных.
    /// </summary>
    /// <param name="config">
    /// Конфигурация контекста базы данных.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если конфигурация или строка подключения равны null.
    /// </exception>
    public DataSourceProvider(IOptions<DatabaseContextConfiguration> config)
    {
        ArgumentNullException.ThrowIfNull(config);

        _defaultDataSource = GetDataSource(config.Value.DefaultConnection);
    }

    /// <inheritdoc />
    public DbDataSource GetDataSource()
    {
        return _defaultDataSource;
    }

    private static NpgsqlDataSource GetDataSource(string conntectionString)
    {
        return new NpgsqlDataSourceBuilder(conntectionString)
            .MapEnum<Status>()
            .MapEnum<Currency>()
            .MapEnum<TransactionType>()
            .Build();
    }

    private readonly NpgsqlDataSource _defaultDataSource;
}
