using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Accounts.Models.Exceptions;

/// <summary>
/// Исключение, возникающее при нарушении бизнес-правил.
/// </summary>
public sealed class AccountDomainException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения с указанным сообщением.
    /// </summary>
    /// <param name="message">
    /// Сообщение, описывающее причину возникновения ошибки.
    /// </param>
    public AccountDomainException(string message) : base(message) { }
}