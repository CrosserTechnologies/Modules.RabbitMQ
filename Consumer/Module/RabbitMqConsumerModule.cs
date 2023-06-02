namespace Crosser.EdgeNode.Modules.RabbitMq.Consumer;

using Crosser.EdgeNode.Common.Abstractions.Utilities.Errors;

public class RabbitMqConsumerModule : FlowModule<RabbitMqConsumerModuleSettings>
{
    public override string UserFriendlyName => "RabbitMQ Consumer";

    private RabbitMqClient? client;

    private RabbitMqClient GetClient => client ?? throw new NullReferenceException("RabbitMq client was null");

    public RabbitMqConsumerModule() : base(FlowModuleType.Input)
    {
    }

    protected override async Task MessageReceived(IFlowMessage message)
    {
        await this.Next(message);
    }

    public override Task<IError> Initialize()
    {
        this.client = new RabbitMqClient(this.Settings);
        this.client.OnDisconnected += (_, _) =>
        {
            if (this.client.ShutDownInitiated is false)
            {
                this.SetStatus(Status.Warning, $"Disconnected from RabbitMQ");
            }
        };
        this.GetClient.OnMessageReceived += async (object? sender, BasicDeliverEventArgs ea) =>
        {
            var rabbitMessage = new FlowMessage();
            rabbitMessage.Set("body", ea.Body.ToArray());
            rabbitMessage.Set("routingKey", ea.RoutingKey);
            rabbitMessage.Set("redelivered", ea.Redelivered);
            rabbitMessage.Set("exchange", ea.Exchange);
            rabbitMessage.Set("deliveryTag", ea.DeliveryTag);
            rabbitMessage.Set("consumerTag", ea.ConsumerTag);
            var message = new FlowMessage();
            message.Set(this.Settings.TargetProperty, rabbitMessage);
            await this.Receive(message);
        };
        return base.Initialize();
    }

    public override Task<IError> Start()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                var subscriptionResult = this.GetClient.Subscribe();
                if (subscriptionResult.IsError)
                {
                    this.SetStatus(Status.Warning, $"RabbitMQ: {subscriptionResult.Error.Message}");
                    await Task.Delay(5000);
                    continue;
                }
                else
                {
                    this.SetStatus(Status.Ok, $"RabbitMQ Connected");
                    break;
                }

            }

        });
        return base.Initialize();
    }
}
