namespace RiskEvaluator.Grpc.Services.Rules;

public class MembershipRule(bool premiumMembershipFailure) : IRule
{
    public int Evaluate(RiskEvaluationRequest request)
    {
        if (!premiumMembershipFailure) return 0;

        return request.Membership switch
        {
            MembershipLevel.Premium => throw new Exception("Random failure in MembershipRule."),
            _ => 0
        };
    }
}