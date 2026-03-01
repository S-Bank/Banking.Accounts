using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Abstractions.Infrastructure.Transport.Serializer;
using Banking.Accounts.Models.DomainEvents;
using Banking.Accounts.Models.Outbox;
using Banking.Accounts.Models.Transaction;
using MediatR;

namespace Banking.Accounts.Logic.DomainEventHandlers;

/// <summary>
/// Обработчик события изменения баланса счета.
/// </summary>
public sealed class AccountBalanceChangedEventHandel : INotificationHandler<AccountBalanceChangedEvent>
{
    /// <summary>
    /// Инициализирует новый экземпляр обработчика события.
    /// </summary>
    /// <param name="unitOfWork">
    /// Единица работы для взаимодействия с БД (запись транзакций и событий).
    /// </param>
    /// <param name="eventSerializer">
    /// Сериализатор для преобразования события в формат сообщения.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если параметры равны null.
    /// </exception>
    public AccountBalanceChangedEventHandel(
        IAccountUnitOfWork unitOfWork,
        ISystemEventSerializer eventSerializer)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(eventSerializer);

        _unitOfWork = unitOfWork;
        _eventSerializer = eventSerializer;
    }

    /// <summary>
    /// Обрабатывает событие изменения баланса.
    /// </summary>
    /// <param name="notification">
    /// Данные события изменения баланса.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Задача, представляющая асинхронную операцию.
    /// </returns>
    public Task Handle(AccountBalanceChangedEvent notification, CancellationToken cancellationToken)
    {
        _unitOfWork.Transactions.Add(
            new Transaction(
                new(Guid.NewGuid()),
                notification.AccountId,
                notification.ReferenceId,
                notification.Amount,
                notification.Type,
                DateTimeOffset.Now));

        var content = _eventSerializer.Serialize(notification);

        _unitOfWork.Outbox.Add(new Outbox(
            new(Guid.NewGuid()),
            notification.AccountId,
            typeof(AccountBalanceChangedEvent).FullName!,
            content,
            DateTimeOffset.UtcNow,
            null,
            null,
            0));


        return Task.CompletedTask;
    }

    private readonly IAccountUnitOfWork _unitOfWork;
    private readonly ISystemEventSerializer _eventSerializer;
}
