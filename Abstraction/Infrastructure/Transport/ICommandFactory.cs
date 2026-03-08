using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Banking.Accounts.Abstractions.Infrastructure.Transport;

public interface ICommandFactory
{
    IRequest CreateCommand(string key, string messageType, string json);
}