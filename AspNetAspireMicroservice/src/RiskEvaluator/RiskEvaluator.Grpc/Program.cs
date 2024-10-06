
using Microsoft.Extensions.Hosting;
using RiskEvaluator.Grpc.Diagnostics;
using RiskEvaluator.Grpc.Services;
using RiskEvaluator.Grpc.Services.Rules;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.AddConsole();

builder.Services.AddGrpc();
builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddSingleton<IRule, AgeRule>();
builder.Services.AddSingleton<IRule, EmailRule>();
builder.Services.AddSingleton<IRule, MembershipRule>(sp => new MembershipRule(
    builder.Configuration.GetValue<bool>("Feature:PremiumMembershipFailure")));

builder.AddServiceDefaults("RiskEvaluator", new BaggageProcessor(), ApplicationDiagnostics.ActivitySourceName);


//builder.AddOpenTelemetry();
var app = builder.Build();

app.MapDefaultEndpoints();

app.MapHealthChecks("/healthz");

app.MapGrpcService<EvaluatorService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();