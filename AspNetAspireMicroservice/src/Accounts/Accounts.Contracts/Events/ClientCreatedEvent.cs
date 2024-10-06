using System.Text.Json.Serialization;

namespace Accounts.Contracts.Events;

public class ClientCreatedEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = default!;
    public string Email { get; init; }= default!;

    public DateOnly Birthdate { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MembershipLevel Membership { get; init; } = MembershipLevel.Regular;
    public List<Address> Addresses { get; set; } = new List<Address>();
}

public enum MembershipLevel
{
    Regular,
    Premium,
}

public record Address(Guid Id, string Street, string City, string State, string Country, string ZipCode);