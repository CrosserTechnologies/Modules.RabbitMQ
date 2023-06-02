namespace Crosser.EdgeNode.Modules.RabbitMq.Common;

public partial class RabbitMqClient
{
    public IResult Publish(ReadOnlyMemory<byte> message)
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
                return Result<IModel>.Fault("Channel could not be created");
            }

            this.Channel.BasicPublish(this.Settings.ExchangeSettings.ExchangeName, this.Settings.ExchangeSettings.ExchangeRoutingKey, false, this.ChannelProperties, message);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fault($"Publish failed: {ex}");
        }
    }
}