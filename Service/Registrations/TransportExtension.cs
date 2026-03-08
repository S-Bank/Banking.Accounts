using Banking.Accounts.Abstractions.Infrastructure.Transport.Processor;
using Banking.Accounts.Abstractions.Infrastructure.Transport.Serializer;
using Banking.Accounts.Infrastructure.Transport.Processor;
using Banking.Accounts.Infrastructure.Transport.Serializer;
using Banking.Accounts.Models.Configurations;
using Banking.Accounts.Service.Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Banking.Accounts.Service.Registrations;

public static class TransportExtension
{
    public static IServiceCollection AddTransport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISystemEventSerializer, SystemEventSerializer>();
        services.AddScoped<ISystemEventProcessor, SystemEventProcessor>();
        services.Configure<KafkaOptions>(configuration.GetSection("Kafka"));

        services.AddSingleton<IProducer<string, string>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<KafkaOptions>>().Value;

            var config = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = true,
                MessageSendMaxRetries = 5
            };

            return new ProducerBuilder<string, string>(config).Build();
        });

        return services;
    }
}
