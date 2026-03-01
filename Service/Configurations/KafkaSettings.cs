namespace Banking.Accounts.Service.Configurations;

/// <summary>
/// Конфигурация подключения к брокеру сообщений Kafka.
/// </summary>
public sealed class KafkaSettings
{
    // <summary>
    /// Список серверов Kafka.
    /// </summary>
    public string BootstrapServers { get; set; } = string.Empty;
}