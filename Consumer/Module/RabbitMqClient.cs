namespace Crosser.EdgeNode.Modules.RabbitMq.Common;

public partial class RabbitMqClient
{
    private IResult<IModel> GetSubscriptionChannel()
    {
        if (this.Channel is not null)
        {
            return Result<IModel>.Ok(this.Channel);
        }

        var channelResult = this.GetChannel();
        if (channelResult.IsError)
        {
            return Result<IModel>.Fault(channelResult.Error.Message);
        }

        if (this.Channel is null)
        {
            return Result<IModel>.Fault("Channel could not be created");
        }


        var settings = (RabbitMqConsumerModuleSettings)this.Settings;
        this.Channel.BasicQos(settings.BasicQoS.PrefetchSize, settings.BasicQoS.PrefetchCount, settings.BasicQoS.Global);

        var _ = this.Channel.QueueDeclare(settings.QueueSettings.QueueName,
                                                     settings.QueueSettings.DurableQueue,
                                                     settings.QueueSettings.ExclusiveQueue,
                                                     settings.QueueSettings.AutoDeleteQueue,
                                                     settings.QueueSettings.QueueDeclarationProperties);

        this.Channel.QueueBind(settings.QueueSettings.QueueName,
                               settings.ExchangeSettings.ExchangeName,
                               settings.ExchangeSettings.ExchangeRoutingKey,
                               settings.ExchangeSettings.ExchangeBindingProperties);

        return Result<IModel>.Ok(this.Channel);
    }

    public IResult Subscribe()
    {
        try
        {
            if (this.Channel is null)
            {
                var channelResult = this.GetSubscriptionChannel();
                if (channelResult.IsError)
                {
                    return Result.Fault(channelResult.Error.Message);
                }
            }

            if (this.Channel is null)
            {
                return Result<IModel>.Fault("Channel could not be created");
            }

            var queueName = this.Channel.QueueDeclare().QueueName;
            var settings = (RabbitMqConsumerModuleSettings)this.Settings;
            this.Channel.QueueBind(settings.QueueSettings.QueueName, this.Settings.ExchangeSettings.ExchangeName, this.Settings.ExchangeSettings.ExchangeRoutingKey, settings.QueueSettings.QueueDeclarationProperties);
            var consumer = new EventingBasicConsumer(this.Channel);
            consumer.Received += (model, ea) =>
            {
                this.OnMessageReceived?.Invoke(this, ea);
            };
            this.Channel.BasicConsume(settings.QueueSettings.QueueName, autoAck: settings.AutoAcknowledge, consumer: consumer);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fault($"Subscribe failed: {ex}");
        }
    }
}