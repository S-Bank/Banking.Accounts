using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Accounts.Models.Exceptions;

public sealed class AccountDomainException : Exception
{
    public AccountDomainException(string message) : base(message) { }
}