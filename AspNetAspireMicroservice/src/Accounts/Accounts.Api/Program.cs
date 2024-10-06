using Accounts.Api.Data;
using Accounts.Api.Events.Handlers.ClientCreated;
using Accounts.Api.Extensions;
using Accounts.Contracts.Events;
using Infrastructure.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);



builder.AddNpgsqlDbContext<AccountsDbContext>("AccountsDb");

builder.AddRabbitMq();

builder.AddServiceDefaults("Accounts", null, RabbitMqDiagnostics.ActivitySourceName);

var app = builder.Build();
await app.Services.InitializeDbAsync();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Accounts");

using var scope = app.Services.CreateScope();
var rabbitMqConsumer = scope.ServiceProvider.GetRequiredService<RabbitMqConsumer<ClientCreatedEvent>>();
rabbitMqConsumer.StartConsuming("clients.events", "accounts.create_account");



app.Run();

