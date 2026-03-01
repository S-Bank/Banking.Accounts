using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Abstractions.Logic.Account;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Banking.Accounts.Logic.Commands;

/// <summary>
/// Обработчик команды открытия нового счета.
/// </summary>
public class OpenAccountCommandHandler : IRequestHandler<OpenAccountCommand, AccountId>
{
    /// <summary>
    /// Инициализирует новый экземпляр обработчика.
    /// </summary>
    /// <param name="unitOfWork">
    /// Единица работы с репозиториями.
    /// </param>
    /// <param name="accountFactory">
    /// Фабрика для создания счетов.
    /// </param>
    /// <param name="logger">
    /// Экземпляр логгера.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
    public OpenAccountCommandHandler(
        IAccountUnitOfWork unitOfWork,
        IAccountFactory accountFactory,
        ILogger<OpenAccountCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(accountFactory);
        ArgumentNullException.ThrowIfNull(logger);

        _unitOfWork = unitOfWork;
        _accountFactory = accountFactory;
        _logger = logger;
    }

    /// <summary>
    /// Обрабатывает команду открытия счета.
    /// </summary>
    /// <param name="request">
    /// Данные для открытия счета.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если объект запроса равен null.
    /// </exception>
    /// <returns>
    /// Идентификатор созданного счета.
    /// </returns>
    public async Task<AccountId> Handle(OpenAccountCommand request, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(request);

        token.ThrowIfCancellationRequested();

        using var scope = _logger.BeginScope("{CustomerId}", request.CustomerId);

        _logger.LogInformation("Начата процедура открытия нового счета.");

        var newAccount = _accountFactory.CreateNew(request.CustomerId, request.Currency);

        _unitOfWork.Accounts.AddAccount(newAccount);

        await _unitOfWork.SaveAsync(token);

        _logger.LogInformation("Счет {AccountId} успешно сохранен.", newAccount.Id);

        return newAccount.Id;
    }

    private readonly IAccountFactory _accountFactory;
    private readonly IAccountUnitOfWork _unitOfWork;
    private readonly ILogger<OpenAccountCommandHandler> _logger;
}
