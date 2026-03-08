using Banking.Accounts.Service.BackgroundJobs;
using Banking.Accounts.Service.Registrations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, lc) => lc
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"));

builder.Services.AddLogic();
builder.Services.AddStorage(builder.Configuration);
builder.Services.AddTransport(builder.Configuration);
builder.Services.AddHostedService<OutboxService>();

var app = builder.Build();

app.Run();