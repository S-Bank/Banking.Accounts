using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;
using Banking.Accounts.Infrastructure.Storage;
using Banking.Accounts.Infrastructure.Storage.Configurations;
using Banking.Accounts.Infrastructure.Storage.Context;
using Banking.Accounts.Infrastructure.Storage.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Banking.Accounts.Service.Registrations;

public static class StorageExtension
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseContextConfiguration>(configuration.GetSection("ConnectionStrings"));
        services.AddDbContext<AccountContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IDataSourceProvider, DataSourceProvider>();
        services.AddScoped<IRepositoryContext, RepositoryContext>();
        services.AddScoped<IAccountUnitOfWork, AccountUnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}