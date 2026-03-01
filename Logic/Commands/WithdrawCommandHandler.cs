using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Banking.Accounts.Logic.Commands;

public sealed class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, Balance>
{

    public WithdrawCommandHandler(
        IAccountUnitOfWork unitOfWork,
        IPublisher publisher,
        ILogger<WithdrawCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(publisher);
        ArgumentNullException.ThrowIfNull(logger);

        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _logger = logger;
    }
    public async Task<Balance> Handle(WithdrawCommand request, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(request);
        token.ThrowIfCancellationRequested();

        using var scope = _logger.BeginScope("{AccountId}", request.AccountId);

        if (await _unitOfWork.Transactions.ExistsAsync(request.ReferenceId, token))
        {
            _logger.LogInformation("Команда с ReferenceId {ReferenceId} уже была обработана.", request.ReferenceId);

            var existingAccount = await _unitOfWork.Accounts.FindAsync(request.AccountId, token);

            return existingAccount!.Balance;
        }

        _logger.LogInformation("Начата списание со счета.");

        var account = await _unitOfWork.Accounts.FindAsync(request.AccountId, token);

        if (account is null)
        {
            _logger.LogWarning("Счет с идентификатором {AccountId} не найден.", request.AccountId);

            throw new InvalidOperationException($"Счет с идентификатором {request.AccountId} не найден.");
        }


        account.Withdraw(request.Amount, request.ReferenceId);

        await _unitOfWork.Accounts.UpdateAsync(account, token);

        foreach (var domainEvent in account.DomainEvents)
        {
            await _publisher.Publish(domainEvent, token);
        }

        account.ClearDomainEvents();

        await _unitOfWork.SaveAsync(token);

        _logger.LogInformation("Со счета {AccountId} успешно списано.", account.Id);

        return account.Balance;

    }

    private readonly IAccountUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly ILogger<WithdrawCommandHandler> _logger;
}
