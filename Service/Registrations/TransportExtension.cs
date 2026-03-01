using Banking.Accounts.Abstractions.Infrastructure.Storage;
using Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;
using Banking.Accounts.Abstractions.Infrastructure.Transport.Serializer;
using Banking.Accounts.Infrastructure.Storage;
using Banking.Accounts.Infrastructure.Storage.Context;
using Banking.Accounts.Infrastructure.Storage.Repositories;
using Banking.Accounts.Infrastructure.Transport.Serializer;
using Banking.Accounts.Service.BackgroundJobs;

namespace Banking.Accounts.Service.Registrations;

public static class TransportExtension
{
    public static IServiceCollection AddTransport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISystemEventSerializer, SystemEventSerializer>();
        services.AddScoped<ISystemEventPublisher, SystemEventPublisher>();

        return services;
    }
}
