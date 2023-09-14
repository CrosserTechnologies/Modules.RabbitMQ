using Crosser.EdgeNode.Modules.RabbitMq.Common;

namespace Crosser.EdgeNode.Modules.RabbitMq.IntegrationTests;

/// <summary>
/// For these tests to run you need a local RabbitMq server
/// </summary>
public class RabbitMqIntegrationTests
{
    [Fact]
    public void CanConnect()
    {
        var settings = new RabbitMqConsumerModuleSettings
        {
            Hostname = "localhost",
            VirtualHost = "/",
            Port = 5672,
            Credential = Guid.NewGuid()
        };
        settings.Credentials.Add(settings.Credential.Value, new CredentialWithUsernamePassword() { Username = "theuser", Password = "thepassword" });

        var client = new RabbitMqClient(settings);
        var connectionResult = client.GetConnection();

        Assert.True(connectionResult.IsSuccess);
        Assert.NotNull(client.Connection);
        Assert.True(client.Connection?.IsOpen);

        client.Connection?.Close();
    }

    [Fact]
    public void CanCreateChannel()
    {
        var settings = new RabbitMqConsumerModuleSettings
        {
            Hostname = "localhost",
            VirtualHost = "/",
            Port = 5672,
            Credential = Guid.NewGuid()
        };
        settings.Credentials.Add(settings.Credential.Value, new CredentialWithUsernamePassword() { Username = "theuser", Password = "thepassword" });
        settings.QueueSettings.QueueName = "test";
        settings.ExchangeSettings.ExchangeName = "test";
        settings.ExchangeSettings.ExchangeRoutingKey = "test";
        settings.ExchangeSettings.ExchangeTypeValue = ExchangeTypeValue.Direct;

        var client = new RabbitMqClient(settings);
        var channelResult = client.GetChannel();

        Assert.True(channelResult.IsSuccess);
        Assert.NotNull(client.Connection);
        Assert.NotNull(client.Channel);
        Assert.True(client.Connection?.IsOpen);

        client.Connection?.Close();
        client.Channel?.Close();
    }

    [Fact]
    public void CanSubscribe()
    {
        var settings = new RabbitMqConsumerModuleSettings
        {
            Hostname = "localhost",
            VirtualHost = "/",
            Port = 5672,
            Credential = Guid.NewGuid()
        };
        settings.Credentials.Add(settings.Credential.Value, new CredentialWithUsernamePassword() { Username = "theuser", Password = "thepassword" });
        settings.QueueSettings.QueueName = "queueY";
        settings.ExchangeSettings.ExchangeName = "exchangeY";
        settings.ExchangeSettings.ExchangeRoutingKey = "routingY";
        settings.ExchangeSettings.ExchangeTypeValue = ExchangeTypeValue.Direct;

        var client = new RabbitMqClient(settings);

        var subResult = client.Subscribe();
        Assert.True(subResult.IsSuccess);

        Assert.NotNull(client.Connection);
        Assert.NotNull(client.Channel);
        Assert.True(client.Connection?.IsOpen);

        client.Connection?.Close();
        client.Channel?.Close();
    }
}