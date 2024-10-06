namespace RiskEvaluator.Grpc.Services.Rules;

public class AgeRule(TimeProvider timeProvider) : IRule
{
    public int Evaluate(RiskEvaluationRequest request)
    {
        var age = GetAge(request);

        return age switch
        {
            < 18 => 30,
            < 23 => 5,
            _ => 0
        };
    }

    private int GetAge(RiskEvaluationRequest request)
    {
        var today = timeProvider.GetUtcNow();
        var birthdate = request.Birthdate.ToDateTime();
        var age = today.Year - birthdate.Year;

        if (!BirthdateHasOccurredThisYear(birthdate, today, age)) 
            age--;

        return age;
    }

    private static bool BirthdateHasOccurredThisYear(DateTime birthdate, DateTimeOffset today, int age)
    {
        return birthdate.Date <= today.AddYears(-age);
    }
}