using System.Diagnostics;

namespace RiskEvaluator.Grpc.Services.Rules;

public class EmailRule : IRule
{
    private readonly List<string> _suspiciousDomains =
    [
        "hotmail.com",
        "gmail.com"
    ];

    public int Evaluate(RiskEvaluationRequest request)
    {
        var emailDomain = GetEmailDomain(request.Email);
        return IsSuspiciousDomain(emailDomain) ? 20 : 0;
    }

    private bool IsSuspiciousDomain(string emailDomain) 
        => _suspiciousDomains.Contains(emailDomain);

    private string GetEmailDomain(string email)
    {
        var atIndex = email.LastIndexOf('@');
        return atIndex == -1 ? string.Empty : email[(atIndex + 1)..];
    }
}