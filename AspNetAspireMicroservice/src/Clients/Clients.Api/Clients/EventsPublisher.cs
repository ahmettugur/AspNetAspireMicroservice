using Clients.Api.Domain.Clients;
using Clients.Contracts.Events;
using Infrastructure.RabbitMQ;

namespace Clients.Api.Clients;
internal class EventsPublisher(RabbitMqPublisher rabbitMqPublisher, bool simulateFailure)
{
    private static readonly Random Random = new();

    public void Publish(ClientCreatedEvent client)
    {
        if (simulateFailure)
        {
            var chance = Random.Next(100);
            if (chance < 25)
            {
                throw new EventPublishException("Simulated failure. Can't publish the event.");
            }
        }

        rabbitMqPublisher.Publish(client, "clients.events");
    }
}

public class EventPublishException(string message) : Exception(message)
{
}
