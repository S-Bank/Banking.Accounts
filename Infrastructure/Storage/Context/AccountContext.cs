using Banking.Accounts.Infrastructure.Storage.Models;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Outbox;
using Banking.Accounts.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Outbox = Banking.Accounts.Infrastructure.Storage.Models.Outbox;
using Transaction = Banking.Accounts.Infrastructure.Storage.Models.Transaction;

namespace Banking.Accounts.Infrastructure.Storage.Context;

/// <summary>
/// Контекст базы данных.
/// </summary>
public sealed class AccountContext : DbContext
{
    /// <summary>
    /// Набор данных банковских счетов.
    /// </summary>
    public DbSet<Account> Accounts { get; init; } = null!;

    /// <summary>
    /// Набор данных транзакций по счетам.
    /// </summary>
    public DbSet<Models.Transaction> Transactions { get; init; } = null!;

    /// <summary>
    /// Набор данных для отправки.
    /// </summary>
    public DbSet<Models.Outbox> Outboxs { get; init; } = null!;

    /// <summary>
    /// Инициализирует новый экземпляр контекста.
    /// </summary>
    /// <param name="options">
    /// Параметры конфигурации контекста.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные параметры (options) равны null.
    /// </exception>
    public AccountContext(DbContextOptions<AccountContext> options) : base(options)
    {
    }

    /// <summary>
    /// Настраивает параметры подключения к базе данных.
    /// </summary>
    /// <param name="optionsBuilder">
    /// Строитель параметров конфигурации.
    /// </param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(x =>
            x.MapEnum<Status>()
            .MapEnum<Currency>()
            .MapEnum<TransactionType>()
        );
    }

    /// <summary>
    /// Выполняет конфигурацию моделей сущностей.
    /// </summary>
    /// <param name="modelBuilder">
    /// Строитель моделей базы данных.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureAccount(modelBuilder);
        ConfigureTransaction(modelBuilder);
        ConfigureOutbox(modelBuilder);
    }

    private static void ConfigureAccount(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Account>()
            .HasKey(a => a.Id);

        _ = modelBuilder.Entity<Account>()
            .Property(a => a.Id)
            .HasConversion<IdValueConverter<AccountId>>()
            .IsRequired();

        _ = modelBuilder.Entity<Account>()
            .Property(a => a.CustomerId)
            .HasConversion<IdValueConverter<CustomerId>>()
            .IsRequired();

        _ = modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasConversion(_balanceConverter)
            .IsRequired();

        _ = modelBuilder.Entity<Account>()
            .Property(a => a.Version)
            .IsConcurrencyToken()
            .HasDefaultValue(1)
            .IsRequired();
    }

    private static void ConfigureTransaction(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Transaction>()
            .HasKey(t => t.Id);

        _ = modelBuilder.Entity<Transaction>()
            .Property(t => t.Id)
            .HasConversion<IdValueConverter<TransactionId>>()
            .IsRequired();

        _ = modelBuilder.Entity<Transaction>()
            .Property(t => t.AccountId)
            .HasConversion<IdValueConverter<AccountId>>()
            .IsRequired();

        _ = modelBuilder.Entity<Transaction>()
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        _ = modelBuilder.Entity<Transaction>()
            .Property(t => t.ReferenceId)
            .HasConversion<IdValueConverter<ReferenceId>>()
            .IsRequired();

        modelBuilder.Entity<Transaction>()
            .ComplexProperty(t => t.Amount, builder =>
        {
            builder.Property(a => a.Units)
                .HasColumnName("amount")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(a => a.Currency)
                .HasColumnName("currency")
                .IsRequired();
        });

        _ = modelBuilder.Entity<Transaction>()
           .Property(t => t.CreatedAt)
           .HasConversion(
                v => v.UtcDateTime, 
                v => v)
           .IsRequired();

        _ = modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.AccountId);

        _ = modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.ReferenceId)
            .IsUnique();

        _ = modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.CreatedAt);

        _ = modelBuilder.Entity<Transaction>()
            .HasIndex(t => new { t.AccountId, t.CreatedAt });

    }

    private static void ConfigureOutbox(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Outbox>()
            .HasKey(a => a.Id);

        _ = modelBuilder.Entity<Outbox>()
            .Property(a => a.Id)
            .HasConversion<IdValueConverter<OutboxId>>()
            .IsRequired();

        _ = modelBuilder.Entity<Outbox>()
            .Property(t => t.AccountId)
            .HasConversion<IdValueConverter<AccountId>>()
            .IsRequired();

        _ = modelBuilder.Entity<Outbox>()
            .Property(o => o.Type)
            .IsRequired();

        _ = modelBuilder.Entity<Outbox>()
            .Property(o => o.Content)
            .HasColumnType("jsonb")
            .IsRequired();

        _ = modelBuilder.Entity<Outbox>()
            .Property(o => o.Error)
            .HasMaxLength(2000)
            .IsRequired(false);

        _ = modelBuilder.Entity<Outbox>()
            .Property(o => o.ErrorCount)
            .HasDefaultValue(0)
            .IsRequired();

        _ = modelBuilder.Entity<Outbox>()
            .Property(o => o.OccurredOn)
            .HasConversion(
                v => v.UtcDateTime,
                v => v.ToUniversalTime()
            )
            .IsRequired();

        _ = modelBuilder.Entity<Outbox>()
            .Property(o => o.ProcessedOn)
            .HasConversion(
                v => v.HasValue ? v.Value.UtcDateTime : (DateTimeOffset?)null,
                v => v)
            .IsRequired(false);

        _ = modelBuilder.Entity<Outbox>()
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Outbox>()
            .HasIndex(o => new { o.ProcessedOn, o.OccurredOn })
            .HasFilter("\"ProcessedOn\" IS NULL");
    }



    #region Converters

    private static readonly ValueConverter<Balance, long> _balanceConverter =
    new(v => v.Value, v => new Balance(v));

    #endregion
}
