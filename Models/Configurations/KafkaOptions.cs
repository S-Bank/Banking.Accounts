namespace Banking.Accounts.Models.Configurations;

public sealed class KafkaOptions
{
    public const string SectionName = "Kafka";

    public required string BootstrapServers { get; init; }

    public required ConsumerSettings AccountCommands { get; init; }
}

public sealed class ConsumerSettings
{
    public required string Topic { get; init; }
    public required string GroupId { get; init; }
}
