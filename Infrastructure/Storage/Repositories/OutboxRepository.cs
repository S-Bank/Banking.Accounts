using Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;
using Banking.Accounts.Infrastructure.Storage.Context;
using Banking.Accounts.Models.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Banking.Accounts.Infrastructure.Storage.Repositories;

// <summary>
/// Реализация репозитория для управления сообщениями.
/// </summary>
public sealed class OutboxRepository : IOutboxRepository
{
    /// <summary>
    /// Инициализирует новый экземпляр репозитория.
    /// </summary>
    /// <param name="context">
    /// Контекст репозитория для доступа к базе данных.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
    public OutboxRepository(
        IRepositoryContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    /// <inheritdoc />
    public void Add(Outbox outbox)
    {
        _context.Outbox.Add(new Models.Outbox
        {
            Id = outbox.Id,
            AccountId = outbox.AccountId,
            Type = outbox.Type,
            Content = outbox.Content,
            OccurredOn = outbox.OccurredOn,
            ProcessedOn = outbox.ProcessedOn,
            Error = outbox.Error,
            ErrorCount = outbox.ErrorCount
        });
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Outbox>> GetUnprocessedAsync(int batchSize, CancellationToken ct)
    {
        var dbEntries = await _context.Outbox
            .Where(o => o.ProcessedOn == null && o.ErrorCount < 5)
            .OrderBy(o => o.OccurredOn)
            .Take(batchSize)
            .ToListAsync(ct);

        return dbEntries.Select(MapToDomain).ToList();
    }

    /// <inheritdoc />
    public void Update(Outbox message)
    {
        var dbEntry = _context.Outbox.Local.FirstOrDefault(x => x.Id == message.Id);

        if (dbEntry != null)
        {
            dbEntry.ProcessedOn = message.ProcessedOn;
            dbEntry.Error = message.Error;
            dbEntry.ErrorCount = message.ErrorCount;
        }
    }

    private readonly IRepositoryContext _context;

    private Outbox MapToDomain(Models.Outbox db) =>
        new(db.Id, db.AccountId, db.Type, db.Content, db.OccurredOn, db.ProcessedOn, db.Error, db.ErrorCount);
}
