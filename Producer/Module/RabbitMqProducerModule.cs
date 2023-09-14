namespace Crosser.EdgeNode.Modules.RabbitMq.Producer;

using Crosser.EdgeNode.Common.Abstractions.Utilities.Errors;

public class RabbitMqProducerModule : FlowModule<RabbitMqProducerModuleSettings>
{
    public override string UserFriendlyName => "RabbitMQ Producer";

    private Regex RegexHasTemplateSyntax = new("^{.*}$");
    private bool ReadRoutingKeyFromMessage;
    private string ErrorSourcePropertyMissing => $"RabbitMQ: The source property '{this.Settings.SourceProperty}' was not present on the incoming message";
    private const string ErrorDataToSendWasNull = "RabbitMQ: Data to send to RabbitMQ was null";
    private string ErrorFailedToGetRoutingKeyFromMessage => $"RabbitMQ: Failed to get routing key '{this.Settings.ExchangeSettings.ExchangeRoutingKey}' from message";
    private string ErrorRabbitMqPublishError(IResult result) => $"RabbitMQ: {result.Error.Message}";
    private RabbitMqClient? client;
    private RabbitMqClient GetClient => client ?? throw new NullReferenceException("RabbitMQ client was null");

    public RabbitMqProducerModule() : base(FlowModuleType.Output)
    {
    }

    protected override async Task MessageReceived(IFlowMessage message)
    {
        if (message.Has(this.Settings.SourceProperty) is false)
        {
            SetStatusWarningAndMessageError(message, ErrorSourcePropertyMissing);
            await this.Next(message);
            return;
        }

        var source = message.Get<object>(this.Settings.SourceProperty);
        var data = source switch
        {
            IFlowMessage flowMessage => flowMessage.ToString(),
            null => null,
            object any => $"{any}"
        };

        if (data is null)
        {
            SetStatusWarningAndMessageError(message, ErrorDataToSendWasNull);
            await this.Next(message);
            return;
        }

        var routingKeyResult = this.GetRoutingKey(message);

        if (routingKeyResult.IsError)
        {
            SetStatusWarningAndMessageError(message, ErrorFailedToGetRoutingKeyFromMessage);
            await this.Next(message);
            return;
        }

        var result = this.GetClient.Publish(Encoding.UTF8.GetBytes(data), routingKeyResult.Value);
        if (result.IsError)
        {
            SetStatusWarningAndMessageError(message, ErrorRabbitMqPublishError(result));
        }
        else
        {
            this.SetStatus(Status.Ok);
            message.SetSuccess();
        }

        await this.Next(message);
    }

    private void SetStatusWarningAndMessageError(IFlowMessage message, string error)
    {
        this.SetStatus(Status.Warning, error);
        message.SetError(error);
    }

    private EdgeNode.Common.Abstractions.Utilities.IResult<string?> GetRoutingKey(IFlowMessage message)
    {
        if (this.ReadRoutingKeyFromMessage)
        {
            return message.GetByTemplate(this.Settings.ExchangeSettings.ExchangeRoutingKey);
        }
        return EdgeNode.Common.Abstractions.Utilities.Result<string?>.Ok(this.Settings.ExchangeSettings.ExchangeRoutingKey);
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

        if (this.Settings.ExchangeSettings.ExchangeRoutingKey is not null &&
        this.RegexHasTemplateSyntax.IsMatch(this.Settings.ExchangeSettings.ExchangeRoutingKey))
        {
            this.ReadRoutingKeyFromMessage = true;
        }
        return base.Initialize();
    }
}
