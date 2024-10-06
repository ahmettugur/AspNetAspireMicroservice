using Accounts.Api.Data;
using Accounts.Api.Domain;
using Accounts.Contracts.Events;
using Infrastructure.RabbitMQ;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Api.Events.Handlers.ClientCreated;

public class ClientCreatedEventHandler(ILogger<ClientCreatedEventHandler> logger,AccountsDbContext dbContext,RabbitMqPublisher publisher): IEventHandler<ClientCreatedEvent>
{
    public async Task HandleAsync(ClientCreatedEvent @event)
    {
        logger.LogInformation("Client created event received: {ClientId}", @event.Id);
        
        var account = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.ClientId == @event.Id);

        if (account is null)
            account = await CreateAccount(@event);

        var accountCreatedEvent = new AccountCreatedEvent(account.Id, account.ClientId,
            account.ClientName, account.ClientEmail);
        
        publisher.Publish(accountCreatedEvent, "accounts.events");
    }
    
    private async Task<Account> CreateAccount(ClientCreatedEvent @event)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            ClientId = @event.Id,
            ClientName = @event.Name,
            ClientEmail = @event.Email
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();
        return account;
    }
}