using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Models.Commands;
using Banking.Accounts.Models.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Banking.Accounts.Logic.Commands;

/// <summary>
/// Обработчик команды списания средств со счета.
/// </summary>
public sealed class WithdrawCommandHandler : IRequestHandler<WithdrawCommand>
{
    /// <summary>
    /// Инициализирует новый экземпляр обработчика команды списания средств.
    /// </summary>
    /// <param name="unitOfWork">
    /// Единица работы для управления транзакциями и доступа к репозиториям.
    /// </param>
    /// <param name="publisher">
    /// Издатель для рассылки доменных событий.
    /// </param>
    /// <param name="logger">
    /// Экземпляр логгера для трассировки процесса списания.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
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

    /// <summary>
    /// Выполняет процесс списания средств с проверкой баланса и обеспечением идемпотентности.
    /// </summary>
    /// <param name="commandParameters">
    /// Команда, содержащая идентификатор счета, сумму списания и уникальный идентификатор операции.
    /// </param>
    /// <param name="token">
    /// Токен отмены асинхронной операции.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если объект запроса равен null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если счет не найден или на балансе недостаточно средств для списания.
    /// </exception>
    /// <exception cref="AccountDomainException">
    /// Выбрасывается при нарушении бизнес-правил агрегата (неактивный статус или конфликт валют).
    /// </exception>
    /// <returns>
    /// Обновленный баланс счета после успешного завершения транзакции.
    /// </returns>
    public async Task Handle(WithdrawCommand commandParameters, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(commandParameters);
        token.ThrowIfCancellationRequested();

        using var scope = _logger.BeginScope("{AccountId}", commandParameters.AccountId);

        _logger.LogInformation("Начата списание со счета.");

        var account = await _unitOfWork.Accounts.FindAsync(commandParameters.AccountId, token);

        if (account is null)
        {
            _logger.LogWarning("Счет с идентификатором {AccountId} не найден.", commandParameters.AccountId);

            throw new InvalidOperationException($"Счет с идентификатором {commandParameters.AccountId} не найден.");
        }

        account.Withdraw(commandParameters.Amount, commandParameters.ReferenceId);

        await _unitOfWork.Accounts.UpdateAsync(account, token);

        foreach (var domainEvent in account.DomainEvents)
        {
            await _publisher.Publish(domainEvent, token);
        }

        account.ClearDomainEvents();

        await _unitOfWork.SaveAsync(token);

        _logger.LogInformation("Со счета {AccountId} успешно списано.", account.Id);
    }

    private readonly IAccountUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly ILogger<WithdrawCommandHandler> _logger;
}
