namespace Infrastructure.RabbitMQ;

public interface IEventHandler<in T>
{
    public Task HandleAsync(T @event);
}