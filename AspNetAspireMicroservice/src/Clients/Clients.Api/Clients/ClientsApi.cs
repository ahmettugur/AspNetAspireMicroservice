using System.Diagnostics;
using Clients.Api.Clients.Risk;
using Clients.Api.Data;
using Clients.Api.Diagnostics.Extensions;
using Clients.Api.Domain.Clients;
using Clients.Contracts.Events;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using MembershipLevel = Clients.Api.Domain.Clients.MembershipLevel;

namespace Clients.Api.Clients;

public class ClientsApi
{
    private const string ClientIdBaggageKey = "client.id";
    private const string NameRequiredMessage = "Name is required.";
    private const string EmailRequiredMessage = "Email is required.";
    private const string DuplicateEmailMessage = "A client with the same email already exists.";
    private const string RiskLevelUnacceptableMessage = "The request cannot be processed. Please contact support.";


    public static RouteGroupBuilder MapClients(IEndpointRouteBuilder routes, ILogger<ClientsApi> logger)
    {
        var group = routes.MapGroup("/clients");

        group.WithTags("Clients");

        group.MapPost("/",
            async (Client newClient, IRiskValidator riskValidator, ClientsDbContext db, EventsPublisher eventsPublisher) =>
            {
                if (string.IsNullOrWhiteSpace(newClient.Name))
                    return Results.BadRequest(NameRequiredMessage);

                if (string.IsNullOrWhiteSpace(newClient.Email))
                    return Results.BadRequest(EmailRequiredMessage);

                if (await IsDuplicatedEmailAsync(db, newClient))
                    return TypedResults.Conflict(DuplicateEmailMessage);

                var client = new Client
                {
                    Name = newClient.Name,
                    Email = newClient.Email,
                    Membership = newClient.Membership,
                    Addresses = newClient.Addresses
                };

                Activity.Current.EnrichWithClient(client);
                Baggage.SetBaggage(ClientIdBaggageKey, client.Id.ToString());

                if (!await riskValidator.HasAcceptableRiskLevelAsync(client))
                    return Results.BadRequest(RiskLevelUnacceptableMessage);

                db.Clients.Add(client);
                await db.SaveChangesAsync();

                var clientCreatedEvent = new ClientCreatedEvent
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    Membership = (Contracts.Events.MembershipLevel)client.Membership,
                };

                logger.LogInformation("Client created with ClinttId: {ClientId}", client.Id);

                eventsPublisher.Publish(clientCreatedEvent);

                return Results.Created($"/clients/{client.Id}", client);
            });

        return group;
    }
    
    private static async Task<bool> IsDuplicatedEmailAsync(ClientsDbContext db, Client newClient)
    {
        return await db.Clients.AnyAsync(c => c.Email == newClient.Email);
    }
}
