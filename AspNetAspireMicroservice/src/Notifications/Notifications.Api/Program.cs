using Accounts.Api.Extensions;
using Infrastructure.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Notifications.Contracts.Events;

var builder = WebApplication.CreateBuilder(args);



builder.Logging.AddConsole();



builder.AddRabbitMq();

builder.AddServiceDefaults("Notifications", null, RabbitMqDiagnostics.ActivitySourceName);


var app = builder.Build();

app.MapDefaultEndpoints();

app.MapHealthChecks("/healthz");

app.MapGet("/", () => "Notifications");

using var scope = app.Services.CreateScope();
var rabbitMqConsumer = scope.ServiceProvider.GetRequiredService<RabbitMqConsumer<AccountCreatedEvent>>();
rabbitMqConsumer.StartConsuming("accounts.events", "notifications.email_sender");

app.Run();