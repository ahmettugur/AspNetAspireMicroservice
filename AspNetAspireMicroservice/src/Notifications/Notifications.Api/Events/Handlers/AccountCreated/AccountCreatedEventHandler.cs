using Infrastructure.RabbitMQ;
using Notifications.Contracts.Events;

namespace Notifications.Api.Events.Handlers.AccountCreated;


public class AccountCreatedEventHandler(ILogger<AccountCreatedEventHandler> logger): IEventHandler<AccountCreatedEvent>
{
    public Task HandleAsync(AccountCreatedEvent @event)
    {
        logger.LogInformation("Sending notification to {Email}. New Client with ID {ClientId} New Account with ID {AccountId}", @event.ClientEmail, @event.ClientId, @event.AccountId);
        return Task.CompletedTask;
    }
}