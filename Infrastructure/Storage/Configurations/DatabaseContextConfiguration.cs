using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Accounts.Infrastructure.Storage.Configurations;

/// <summary>
/// Конфигурация контекста базы данных.
/// </summary>
public sealed class DatabaseContextConfiguration
{
    /// <summary>
    /// Строка подключения к базе данных по умолчанию.
    /// </summary>
    public required string DefaultConnection { get; set; }
}
