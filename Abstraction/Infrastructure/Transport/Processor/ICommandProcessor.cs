namespace Banking.Accounts.Abstractions.Infrastructure.Transport.Processor;

public interface ICommandProcessor
{
    Task ProcessAsync(CancellationToken token);
}
