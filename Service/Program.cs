using Banking.Accounts.Service.BackgroundJobs;
using Banking.Accounts.Service.Extension;
using Banking.Accounts.Service.Middlewares;
using Banking.Accounts.Service.Registrations;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, lc) => lc
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banking Accounts API",
        Version = "v1",
        Description = "API для управления банковскими счетами"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    options.AddSecurityDefinition("IdempotencyKey", new OpenApiSecurityScheme
    {
        Name = "X-Idempotency-Key",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Уникальный ключ операции (GUID)"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "IdempotencyKey"
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddLogic();
builder.Services.AddStorage(builder.Configuration);
builder.Services.AddTransport(builder.Configuration);
builder.Services.AddHostedService<OutboxJob>();


var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Banking API";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Accounts API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();