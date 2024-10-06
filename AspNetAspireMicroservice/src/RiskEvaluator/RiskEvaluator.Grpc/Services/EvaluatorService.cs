using System.Diagnostics;
using Grpc.Core;
using OpenTelemetry;
using OpenTelemetry.Trace;
using RiskEvaluator.Grpc.Services.Rules;
using Status = OpenTelemetry.Trace.Status;

namespace RiskEvaluator.Grpc.Services;

public class EvaluatorService(ILogger<EvaluatorService> logger, IEnumerable<IRule> rules) : Evaluator.EvaluatorBase
{
    public override Task<RiskEvaluationReply> Evaluate(RiskEvaluationRequest request, ServerCallContext context)
    {
        try
        {
            var clientId = Baggage.Current.GetBaggage("client.id");
            logger.LogInformation("Evaluating risk for {Email} {ClientId}", request.Email, clientId);
            Activity.Current?.SetTag("client.id", clientId);

            var score = rules.Sum(rule => rule.Evaluate(request));

            var level = score switch
            {
                <= 5 => RiskLevel.Low,
                <= 20 => RiskLevel.Medium,
                _ => RiskLevel.High
            };

            Activity.Current?.SetTag("evaluation.email", request.Email);

            Activity.Current?.AddEvent(
                new ActivityEvent(
                    "RiskResult",
                    tags: new ActivityTagsCollection(
                        new KeyValuePair<string, object?>[]
                        {
                            new("risk.score", score),
                            new("risk.level", level)
                        }
                    )
                )
            );

            return Task.FromResult(new RiskEvaluationReply()
            {
                RiskLevel = level,
            });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occurred during risk evaluation");

            Activity.Current?.SetStatus(Status.Error);
            Activity.Current?.RecordException(exception);

            return Task.FromResult(new RiskEvaluationReply()
            {
                RiskLevel = RiskLevel.High,
            });
        }
    }
}