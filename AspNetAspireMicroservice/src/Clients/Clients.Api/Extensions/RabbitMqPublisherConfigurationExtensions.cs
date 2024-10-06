using Clients.Api.Clients;
using Infrastructure.RabbitMQ;
using RabbitMQ.Client;


namespace Clients.Api.Extensions;

public static class RabbitMqPublisherConfigurationExtensions
{
    public static WebApplicationBuilder AddRabbitMq(this WebApplicationBuilder builder)
    {

        var rabbitmqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>()!;

        builder.AddRabbitMQClient(rabbitmqSettings.Hostname, configureConnectionFactory: cf =>
        {
            cf.UserName = rabbitmqSettings.Username;
            cf.Password = rabbitmqSettings.Password;
            cf.Port = rabbitmqSettings.Port;
        });

        builder.Services.AddSingleton(sp =>
            new RabbitMqPublisher(sp.GetRequiredService<IConnection>()));
        builder.Services.AddSingleton<EventsPublisher>(sp =>
            new EventsPublisher(sp.GetRequiredService<RabbitMqPublisher>(),
                builder.Configuration.GetValue<bool>("Feature:PublishEventFailure")));

        return builder;
    }
}