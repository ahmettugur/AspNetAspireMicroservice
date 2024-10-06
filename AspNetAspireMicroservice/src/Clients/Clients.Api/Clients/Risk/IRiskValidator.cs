using Clients.Api.Domain.Clients;
using Clients.Contracts.Events;
using RiskEvaluator;

namespace Clients.Api.Clients.Risk;

public interface IRiskValidator
{
    Task<bool> HasAcceptableRiskLevelAsync(Client client);
}