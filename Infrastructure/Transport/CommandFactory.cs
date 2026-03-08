using System.Text.Json;
using Banking.Accounts.Abstractions.Infrastructure.Transport;
using Banking.Accounts.Infrastructure.Contracts.Input.Commands;
using MediatR;

using CommandModel = Banking.Accounts.Models.Commands;

namespace Banking.Accounts.Infrastructure.Transport;

public sealed class CommandFactory : ICommandFactory
{
    /// <inheritdoc />
    public IRequest CreateCommand(string key, string messageType, string json)
    {
        return messageType switch
        {
            "Deposit" => MapToDeposit(key, json),
            "Withdraw" => MapToWithdraw(key, json),
            "OpenAccount" => MapToOpenAccount(key, json),
            _ => throw new NotSupportedException($"Тип команды '{messageType}' не поддерживается.")
        };
    }

    private CommandModel.OpenAccountCommand MapToOpenAccount(string key, string json)
    {
        var command = JsonSerializer.Deserialize<OpenAccountCommand>(json)
                  ?? throw new InvalidOperationException("Не удалось десериализовать OpenAccountCommand");

        return new CommandModel.OpenAccountCommand(
            new(command.AccountId),
            new(command.CustomerId),
            command.Currency);
    }

    private CommandModel.DepositCommand MapToDeposit(string key, string json)
    {
        var command = JsonSerializer.Deserialize<DepositCommand>(json)
                  ?? throw new InvalidOperationException("Не удалось десериализовать DepositCommand");

        return new CommandModel.DepositCommand(
            new(Guid.Parse(key)),
            new(command.ReferenceId),
            new(command.Amount.Value,
            command.Amount.Currency)
        );
    }

    private CommandModel.WithdrawCommand MapToWithdraw(string key, string json)
    {
        var command = JsonSerializer.Deserialize<WithdrawCommand>(json)
                  ?? throw new InvalidOperationException("Не удалось десериализовать WithdrawCommand");

        return new CommandModel.WithdrawCommand(
            new(Guid.Parse(key)),
            new(command.ReferenceId),
            new(command.Amount.Value,
            command.Amount.Currency)
        );
    }
}
