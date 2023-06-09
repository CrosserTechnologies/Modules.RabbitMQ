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
        var settings = new RabbitMqProducerModuleSettings
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

        client.Dispose();
    }

    [Fact]
    public void CanCreateChannel()
    {
        var settings = new RabbitMqProducerModuleSettings
        {
            Hostname = "localhost",
            VirtualHost = "/",
            Port = 5672,
            Credential = Guid.NewGuid()
        };
        settings.Credentials.Add(settings.Credential.Value, new CredentialWithUsernamePassword() { Username = "theuser", Password = "thepassword" });
        settings.ExchangeSettings.ExchangeName = "test";
        settings.ExchangeSettings.ExchangeRoutingKey = "test";
        settings.ExchangeSettings.ExchangeTypeValue = ExchangeTypeValue.Direct;

        var client = new RabbitMqClient(settings);
        var channelResult = client.GetChannel();

        Assert.True(channelResult.IsSuccess);
        Assert.NotNull(client.Connection);
        Assert.NotNull(client.Channel);
        Assert.True(client.Connection?.IsOpen);

        client.Dispose();
    }

    [Fact]
    public void CanPublish()
    {
        var settings = new RabbitMqProducerModuleSettings
        {
            Hostname = "localhost",
            VirtualHost = "/",
            Port = 5672,
            Credential = Guid.NewGuid()
        };
        settings.Credentials.Add(settings.Credential.Value, new CredentialWithUsernamePassword() { Username = "theuser", Password = "thepassword" });
        settings.ExchangeSettings.ExchangeName = "testX";
        settings.ExchangeSettings.ExchangeRoutingKey = "testX";
        settings.ExchangeSettings.ExchangeTypeValue = ExchangeTypeValue.Direct;

        var client = new RabbitMqClient(settings);
        byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
        var channelResult = client.Publish(messageBodyBytes);

        Assert.True(channelResult.IsSuccess);
        Assert.NotNull(client.Connection);
        Assert.NotNull(client.Channel);
        Assert.True(client.Connection?.IsOpen);

        client.Dispose();
    }
}