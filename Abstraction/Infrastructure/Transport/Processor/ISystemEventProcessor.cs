using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Accounts.Abstractions.Infrastructure.Transport.Processor;

/// <summary>
/// Описывает компонент для фоновой обработки и публикации системных событий.
/// </summary>
public interface ISystemEventProcessor
{
    /// <summary>
    /// Выполняет процесс чтения необработанных записей из таблицы событий
    /// </summary>
    /// <param name="token">
    /// Токен отмены асинхронной операции.
    /// </param>
    /// <returns>
    /// Задача, представляющая асинхронную операцию обработки.
    /// </returns>
    Task ProcessAsync(CancellationToken token);
}
