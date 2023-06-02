namespace Crosser.EdgeNode.Modules.RabbitMq.Producer;

using Crosser.EdgeNode.Common.Abstractions.Utilities.Errors;

public class RabbitMqProducerModule : FlowModule<RabbitMqProducerModuleSettings>
{
    public override string UserFriendlyName => "RabbitMQ Producer";

    private RabbitMqClient? client;
    private RabbitMqClient GetClient => client ?? throw new NullReferenceException("RabbitMq client was null");

    public RabbitMqProducerModule() : base(FlowModuleType.Output)
    {
    }

    protected override async Task MessageReceived(IFlowMessage message)
    {
        var source = message.Get<object>(this.Settings.SourceProperty);
        var data = source switch
        {
            IFlowMessage flowMessage => flowMessage.ToString(),
            object any => $"{any}"
        };

        if (data is null)
        {
            throw new NullReferenceException("Data to send to RabbitMQ was null");
        }

        var result = this.GetClient.Publish(Encoding.UTF8.GetBytes(data));
        if (result.IsError)
        {
            this.SetStatus(Status.Warning, $"RabbitMQ: {result.Error.Message}");
            message.SetError(result.Error.Message);
        }
        else
        {
            this.SetStatus(Status.Ok);
            message.SetSuccess();
        }
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
        return base.Initialize();
    }
}
