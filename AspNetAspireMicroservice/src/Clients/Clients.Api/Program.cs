using System.Text.Json.Serialization;
using Clients.Api.Clients;
using Clients.Api.Clients.Risk;
using Clients.Api.Data;
using Clients.Api.Diagnostics;
using Clients.Api.Extensions;
using Infrastructure.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RiskEvaluator.Grpc;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddConsole();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.AddNpgsqlDbContext<ClientsDbContext>("ClientsDb");

builder.Services.AddSingleton<IRiskValidator, RiskValidator>();

builder.Services.AddGrpcClient<Evaluator.EvaluatorClient>(options =>
{
    options.Address = new Uri(builder.Configuration["RiskEvaluator:Url"]!);
});

builder.AddServiceDefaults("Clients.Api", null, RabbitMqDiagnostics.ActivitySourceName);

builder.AddRabbitMq();




var app = builder.Build();

app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI();
await app.Services.InitializeDbAsync();

app.UseHttpsRedirection();

var logger = app.Services.GetRequiredService<ILogger<ClientsApi>>();

ClientsApi.MapClients(app, logger);

app.Run();