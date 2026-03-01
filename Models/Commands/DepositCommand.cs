using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Transaction;
using MediatR;

namespace Banking.Accounts.Models.Commands;

/// <summary>
/// Команда на пополнение счета.
/// </summary>
/// <param name="AccountId">
/// Идентификатор счета.
/// </param>
/// <param name="ReferenceId">
/// Идентификатор операции.
/// </param>
/// <param name="Amount">
/// Сумма пополнения.
/// </param>
public sealed record DepositCommand(
    AccountId AccountId,
    ReferenceId ReferenceId,
    Amount Amount)
    : IRequest<Balance>;
