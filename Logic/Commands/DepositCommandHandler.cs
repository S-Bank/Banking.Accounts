using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Commands;
using Banking.Accounts.Models.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Banking.Accounts.Logic.Commands;

/// <summary>
/// Обработчик команды пополнения счета.
/// </summary>
public sealed class DepositCommandHandler : IRequestHandler<DepositCommand, Balance>
{
    /// <summary>
    /// Инициализирует новый экземпляр обработчика.
    /// </summary>
    /// <param name="unitOfWork">
    /// Единица работы с репозиториями.
    /// </param>
    /// <param name="publisher">
    /// Издатель событий.
    /// </param>
    /// <param name="logger">
    /// Экземпляр логгера.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
    public DepositCommandHandler(
        IAccountUnitOfWork unitOfWork,
        IPublisher publisher,
        ILogger<DepositCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(publisher);
        ArgumentNullException.ThrowIfNull(logger);

        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _logger = logger;
    }

    /// <summary>
    /// Обрабатывает запрос на пополнение счета.
    /// </summary>
    /// <param name="request">
    /// Команда, содержащая идентификатор счета и сумму пополнения.
    /// </param>
    /// <param name="token">
    /// Токен отмены операции.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если запрос равен null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если указанный счет не найден.
    /// </exception>
    /// <exception cref="AccountDomainException">
    /// Выбрасывается при нарушении бизнес-правил агрегата (например, несовпадение валют).
    /// </exception>
    /// <returns>
    /// Обновленный баланс счета после завершения транзакции.
    /// </returns>
    public async Task<Balance> Handle(DepositCommand request, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(request);

        token.ThrowIfCancellationRequested();

        using var scope = _logger.BeginScope("{AccountId}", request.AccountId);

        _logger.LogInformation("Начата пополнения счета.");

        var account = await _unitOfWork.Accounts.FindAsync(request.AccountId, token);

        if (account is null)
        {
            _logger.LogWarning("Счет с идентификатором {AccountId} не найден.", request.AccountId);

            throw new InvalidOperationException($"Счет с идентификатором {request.AccountId} не найден.");
        }

        account.Deposit(request.Amount, request.ReferenceId);

        await _unitOfWork.Accounts.UpdateAsync(account, token);

        foreach (var domainEvent in account.DomainEvents)
        {
            await _publisher.Publish(domainEvent, token);
        }

        account.ClearDomainEvents();

        await _unitOfWork.SaveAsync(token);

        _logger.LogInformation("Счет {AccountId} успешно пополнен.", account.Id);

        return account.Balance;
    }

    private readonly IAccountUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly ILogger<DepositCommandHandler> _logger;
}
