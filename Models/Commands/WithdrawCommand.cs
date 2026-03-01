using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Transaction;
using MediatR;

namespace Banking.Accounts.Models.Commands;

public sealed record WithdrawCommand(
    AccountId AccountId,
    ReferenceId ReferenceId,
    Amount Amount)
    : IRequest<Balance>;
