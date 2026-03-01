using Banking.Accounts.Abstractions.Logic.Account;
using Banking.Accounts.Logic.Account;
using Banking.Accounts.Logic.Commands;

namespace Banking.Accounts.Service.Extension;

public static class LogicExtension
{
    public static IServiceCollection AddLogic(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(OpenAccountCommandHandler).Assembly);
        });

        services.AddSingleton<IAccountFactory, AccountFactory>();

        return services;
    }
}
