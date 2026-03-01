using Banking.Accounts.Abstractions.Infrastructure.Transport.Processor;
using Banking.Accounts.Abstractions.Infrastructure.Transport.Serializer;
using Banking.Accounts.Infrastructure.Transport.Processor;
using Banking.Accounts.Infrastructure.Transport.Serializer;
using Banking.Accounts.Service.Configurations;
using Confluent.Kafka;

namespace Banking.Accounts.Service.Registrations;

public static class TransportExtension
{
    public static IServiceCollection AddTransport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISystemEventSerializer, SystemEventSerializer>();
        services.AddScoped<ISystemEventProcessor, SystemEventProcessor>();

        var kafkaConfig = configuration.GetSection("Kafka").Get<KafkaSettings>();

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaConfig!.BootstrapServers
        };

        services.AddSingleton<IProducer<string, string>>(
            new ProducerBuilder<string, string>(producerConfig).Build());

        return services;
    }
}
