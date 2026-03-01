using Banking.Accounts.Models.Outbox;

namespace Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;

public interface IOutboxRepository
{
    void Add(Outbox outbox);

    Task<IReadOnlyCollection<Outbox>> GetUnprocessedAsync(int batchSize, CancellationToken ct);

    void Update(Outbox message);
}
