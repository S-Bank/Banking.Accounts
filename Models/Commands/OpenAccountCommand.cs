using Banking.Accounts.Models.Account;
using MediatR;

namespace Banking.Accounts.Models.Commands;

/// <summary>
/// Команда на добавление счета.
/// </summary>
/// <param name="CustomerId">
/// Идентификатор клиента.
/// </param>
/// <param name="Currency">
/// Валюта счета.
/// </param>
public sealed record OpenAccountCommand(
    CustomerId CustomerId,
    Currency Currency
) : IRequest<AccountId>;
