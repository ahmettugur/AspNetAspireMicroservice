using System.Diagnostics;

namespace RiskEvaluator.Grpc.Diagnostics;

public static class ApplicationDiagnostics
{
    public const string ActivitySourceName = "RiskEvaluator.Application";
    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}