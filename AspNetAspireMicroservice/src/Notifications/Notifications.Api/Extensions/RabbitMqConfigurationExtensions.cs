
using Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Api.Events.Handlers.AccountCreated;
using Notifications.Contracts.Events;
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


        builder.Services.AddScoped<AccountCreatedEventHandler>();

        builder.Services.AddScoped<RabbitMqConsumer<AccountCreatedEvent>>(
            sp => new RabbitMqConsumer<AccountCreatedEvent>
            (sp.GetRequiredService<IConnection>(),sp.GetRequiredService<AccountCreatedEventHandler>()));

        return builder;
    }
}