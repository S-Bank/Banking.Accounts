using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Abstractions.Infrastructure.Transport.Serializer;
using Banking.Accounts.Models.DomainEvents;
using Banking.Accounts.Models.Outbox;
using Banking.Accounts.Models.Transaction;
using MediatR;

namespace Banking.Accounts.Logic.DomainEventHandlers;

public sealed class AccountBalanceChangedEventHandel : INotificationHandler<AccountBalanceChangedEvent>
{
    public AccountBalanceChangedEventHandel(
        IAccountUnitOfWork unitOfWork,
        ISystemEventSerializer eventSerializer)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(eventSerializer);

        _unitOfWork = unitOfWork;
        _eventSerializer = eventSerializer;
    }
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
