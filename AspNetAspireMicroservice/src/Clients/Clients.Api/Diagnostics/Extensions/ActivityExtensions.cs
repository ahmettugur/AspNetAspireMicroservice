using System.Diagnostics;
using Clients.Api.Domain.Clients;

namespace Clients.Api.Diagnostics.Extensions;

public static class ActivityExtensions
{
    public static Activity? EnrichWithClient(this Activity? activity, Client client)
    {
        activity?.SetTag("client.id", client.Id);
        activity?.SetTag("client.membership", client.Membership.ToString());
        return activity;
    }
}