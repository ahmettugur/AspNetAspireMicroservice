
using Accounts.Api.Events.Handlers.ClientCreated;
using Accounts.Contracts.Events;
using Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;


namespace Accounts.Api.Extensions;

public static class RabbitMqConfigurationExtensions
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

        builder.Services.AddScoped<ClientCreatedEventHandler>();

        builder.Services.AddScoped(
            sp => new RabbitMqConsumer<ClientCreatedEvent>
            (sp.GetRequiredService<IConnection>(),sp.GetRequiredService<ClientCreatedEventHandler>()));


        return builder;
    }
}