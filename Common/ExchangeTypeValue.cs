namespace Crosser.EdgeNode.Modules.RabbitMq.Common;

public enum ExchangeTypeValue
{
    //
    // Summary:
    //     Exchange type used for AMQP direct exchanges.
    Direct,
    //
    // Summary:
    //     Exchange type used for AMQP fanout exchanges.
    Fanout,
    //
    // Summary:
    //     Exchange type used for AMQP headers exchanges.
    Headers,
    //
    // Summary:
    //     Exchange type used for AMQP topic exchanges.
    Topic
}
