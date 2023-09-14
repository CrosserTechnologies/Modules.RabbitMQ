namespace Crosser.EdgeNode.Modules.RabbitMq.Common;

public partial class RabbitMqClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">The message to publish</param>
    /// <param name="routingKey">The routing key to use, if null the routing key from ExchangeSettings.ExchangeRoutingKey is used</param>
    /// <returns></returns>
    public IResult Publish(ReadOnlyMemory<byte> message, string? routingKey = null)
    {
        try
        {
            if (this.Channel is null)
            {
                var channelResult = this.GetChannel();
                if (channelResult.IsError)
                {
                    return Result.Fault(channelResult.Error.Message);
                }
            }

            if (this.Channel is null)
            {
                return Result.Fault("Channel could not be created");
            }

            this.Channel.BasicPublish(this.Settings.ExchangeSettings.ExchangeName, routingKey ?? this.Settings.ExchangeSettings.ExchangeRoutingKey, false, this.ChannelProperties, message);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fault($"Publish failed: {ex}");
        }
    }
}