using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Transaction;
using MediatR;

namespace Banking.Accounts.Models.Commands;

/// <summary>
/// Команда для выполнения списания средств с банковского счета.
/// </summary>
/// <param name="AccountId">
/// Уникальный идентификатор счета.
/// </param>
/// <param name="ReferenceId">
/// Уникальный идентификатор операции.
/// </param>
/// <param name="Amount">
/// Сумма и валюта списания.
/// </param>
public sealed record WithdrawCommand(
    AccountId AccountId,
    ReferenceId ReferenceId,
    Amount Amount)
    : IRequest<Balance>;
