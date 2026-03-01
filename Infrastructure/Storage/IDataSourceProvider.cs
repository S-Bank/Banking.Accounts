using System.Data.Common;

namespace Banking.Accounts.Infrastructure.Storage;

/// <summary>
/// Интерфейс провайдера источника данных.
/// </summary>
public interface IDataSourceProvider
{
    /// <summary>
    /// Возвращает настроенный источник данных.
    /// </summary>
    /// <returns>
    /// Источник данных для управления подключениями.
    /// </returns>
    DbDataSource GetDataSource();
}
