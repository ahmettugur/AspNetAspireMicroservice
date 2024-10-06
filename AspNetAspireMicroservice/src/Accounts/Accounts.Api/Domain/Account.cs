namespace Accounts.Api.Domain;

public class Account
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; }
    public string ClientEmail { get; set; } 
}