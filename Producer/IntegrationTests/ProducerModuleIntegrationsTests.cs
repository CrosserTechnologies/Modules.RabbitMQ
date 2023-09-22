using Crosser.EdgeNode.Flows.Models.Abstractions.Models;
using Crosser.EdgeNode.Modules.RabbitMq.Common;
using Crosser.EdgeNode.Modules.Testing;

namespace Crosser.EdgeNode.Modules.RabbitMq.IntegrationTests;

/// <summary>
/// For these tests to run you need a local RabbitMq server
/// </summary>
public class ProducerModuleIntegrationTests
{
    private RabbitMqProducerModule module;

    public ProducerModuleIntegrationTests()
    {
        module = new RabbitMqProducerModule();
        module.Settings.Hostname = "localhost";
        module.Settings.VirtualHost = "/";
        module.Settings.Port = 5672;
        module.Settings.Credential = Guid.NewGuid();
        module.Settings.Credentials.Add(module.Settings.Credential.Value, new CredentialWithUsernamePassword() { Username = "theuser", Password = "thepassword" });
        module.Settings.ExchangeSettings.ExchangeName = "testX";
        module.Settings.ExchangeSettings.ExchangeRoutingKey = "testX";
        module.Settings.ExchangeSettings.ExchangeTypeValue = ExchangeTypeValue.Direct;
    }

    [Fact]
    public void CanPublishWithStaticRoutingKey()
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

    [Fact]
    public async Task CanPublishWithEmptyRoutingKey()
    {
        module.Settings.ExchangeSettings.ExchangeRoutingKey = "";

        var flow = module.SetupFlow();

        var startResult = await flow.Start();
        Assert.True(startResult.IsSuccess);

        var msg = new FlowMessage();
        msg.Set("data", "Hello");
        await flow.Receive(msg);

        var result = await flow.GetNextResult();
        Assert.True(result.IsSuccess);

        var stopResult = await flow.Stop();
        Assert.True(stopResult.IsSuccess);
    }

    [Fact]
    public async Task CanPublishWithRoutingKey()
    {
        var flow = module.SetupFlow();

        var startResult = await flow.Start();
        Assert.True(startResult.IsSuccess);

        var msg = new FlowMessage();
        msg.Set("data", "Hello");
        await flow.Receive(msg);

        var result = await flow.GetNextResult();
        Assert.True(result.IsSuccess);

        var stopResult = await flow.Stop();
        Assert.True(stopResult.IsSuccess);
    }

    [Fact]
    public async Task CanPublishWithoutRoutingKeyAsTemplate()
    {
        module.Settings.ExchangeSettings.ExchangeRoutingKey = "{routing.key}";
        var flow = module.SetupFlow();

        var startResult = await flow.Start();
        Assert.True(startResult.IsSuccess);

        var msg = new FlowMessage();
        msg.Set("data", "Hello");
        msg.Set("routing.key", "textX");
        await flow.Receive(msg);

        var result = await flow.GetNextResult();
        Assert.True(result.IsSuccess);

        var stopResult = await flow.Stop();
        Assert.True(stopResult.IsSuccess);
    }

    [Fact]
    public async Task ShouldGetSuccessFalseAndCorrectErrorWhenSourceIsMissingOnFlowMessage()
    {
        var flow = module.SetupFlow();

        var startResult = await flow.Start();
        Assert.True(startResult.IsSuccess);

        var msg = new FlowMessage();
        await flow.Receive(msg);

        FlowMessage result = (FlowMessage)await flow.GetNextResult();
        Assert.False(result.IsSuccess);
        Assert.Equal("RabbitMQ: The source property 'data' was not present on the incoming message", result.ErrorMessage);

        var stopResult = await flow.Stop();
        Assert.True(stopResult.IsSuccess);
    }

    [Fact]
    public async Task ShouldGetSuccessFalseAndCorrectErrorWhenSourceIsNull()
    {
        var flow = module.SetupFlow();

        var startResult = await flow.Start();
        Assert.True(startResult.IsSuccess);

        var msg = new FlowMessage();
        msg.Set<object>("data", null);
        await flow.Receive(msg);

        FlowMessage result = (FlowMessage)await flow.GetNextResult();
        Assert.False(result.IsSuccess);
        Assert.Equal("RabbitMQ: Data to send to RabbitMQ was null", result.ErrorMessage);

        var stopResult = await flow.Stop();
        Assert.True(stopResult.IsSuccess);
    }

    [Fact]
    public async Task ShouldGetSuccessFalseAndCorrectErrorWhenRoutingKeyIsMissing()
    {
        module.Settings.ExchangeSettings.ExchangeRoutingKey = "{routing.key}";
        var flow = module.SetupFlow(timeout: 10000000);

        var startResult = await flow.Start();
        Assert.True(startResult.IsSuccess);

        var msg = new FlowMessage();
        msg.Set("data", "hello");
        await flow.Receive(msg);

        FlowMessage result = (FlowMessage)await flow.GetNextResult();
        Assert.False(result.IsSuccess);
        Assert.Equal($"RabbitMQ: Failed to get routing key '{module.Settings.ExchangeSettings.ExchangeRoutingKey}' from message", result.ErrorMessage);

        var stopResult = await flow.Stop();
        Assert.True(stopResult.IsSuccess);
    }
}