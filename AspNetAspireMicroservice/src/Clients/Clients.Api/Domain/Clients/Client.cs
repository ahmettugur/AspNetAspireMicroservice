using System.Text.Json.Serialization;

namespace Clients.Api.Domain.Clients;

public class Client
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = default!;
    public string Email { get; init; }= default!;

    public DateOnly Birthdate { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MembershipLevel Membership { get; init; } = MembershipLevel.Regular;
    public List<Address> Addresses { get; set; } = [];
}

public enum MembershipLevel
{
    Regular,
    Premium,
}

public record Address(Guid Id, string Street, string City, string State, string Country, string ZipCode);