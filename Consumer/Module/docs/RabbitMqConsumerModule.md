# RabbitMQ Consumer

Will receive messages from RabbitMQ

## Settings

| Name                   | Requirements                       | Purpose                                                                                                            | Default     |
| ---------------------- | ---------------------------------- | ------------------------------------------------------------------------------------------------------------------ | ----------- |
| Target Property        | String with length `1 to 64`       | The property that will contain the message                                                                         | 'data'      |
| Hostname               | String with length `1 to 256`      | Rabbit server to connect to                                                                                        | 'localhost' |
| Port                   | Number larger than 0               | Port of the RabbitMq server. Normally 5672 for regular (plain TCP) connections, 5671 for connections with TLS      | '5672'      |
| Virtual Host           | String with length `0 to 64`       |                                                                                                                    | '/'         |
| Use TLS                | Boolean                            | Check to enable Transport Layer Security. You might need to change the port when enabled (default is 5671 for TLS) | 'false'     |
| Credential             | Username & Password                |                                                                                                                    | 'false'     |
| Basic QoS              | See Basic QoS section              |                                                                                                                    |             |
| Auto Acknowledge       | Boolean                            |                                                                                                                    | 'true'      |
| Queue Settings         | See Queue Settings section         |                                                                                                                    |             |
| Exchange Settings      | See Exchange Settings section      |                                                                                                                    |             |
| Channel Basic Settings | See Channel Basic Settings section |                                                                                                                    |             |

### Basic QoS

| Name           | Requirements                       | Purpose | Default |
| -------------- | ---------------------------------- | ------- | ------- |
| Prefetch Size  | Number within range `0-4294967295` |         | '0'     |
| Prefetch Count | Number within range `0-65535`      |         | '0'     |
| Global         | Boolean                            |         | 'false' |

### Queue Settings

| Name                   | Requirements                  | Purpose                                                        | Default |
| ---------------------- | ----------------------------- | -------------------------------------------------------------- | ------- |
| Name                   | String with length `1 to 256` | Name of the queue to use or create.                            | ''      |
| Durable                | Boolean                       | Creates a durable queue when selected.                         | 'false' |
| Exclusive              | Boolean                       | Creates an exclusive queue when selected.                      | 'false' |
| Auto Delete            | Boolean                       | Automatically deletes a queue after all consumers unsubscribe. | 'false' |
| Declaration Properties | Key/Value pairs               | Additional queue declaration properties to use.                | ''      |

### Exchange Settings

| Name                   | Requirements                            | Purpose                                                                  | Default  |
| ---------------------- | --------------------------------------- | ------------------------------------------------------------------------ | -------- |
| Name                   | String with length `1 to 256`           | Binding name.                                                            | ''       |
| Type                   | One of `Direct, Fanout, Headers, Topic` | Binding type.                                                            | 'Direct' |
| Durable                | Boolean                                 | Creates a durable exchange when selected.                                | 'false'  |
| Auto Delete            | Boolean                                 | Automatically deletes an exchange when all queues are finished using it. | 'false'  |
| Routing Key            | String with length `0 to 256`           | Leave empty to default to the queue name.                                | ''       |
| Declaration Properties | Key/Value pairs                         | Additional exchange declaration properties to use.                       | ''       |
| Binding Properties     | Key/Value pairs                         | Additional exchange binding properties to use.                           | ''       |

### Channel Basic Settings

| Name             | Requirements    | Purpose | Default |
| ---------------- | --------------- | ------- | ------- |
| Name             | String          |         | ''      |
| Content Encoding | String          |         | ''      |
| Content Type     | String          |         | ''      |
| Correlation Id   | String          |         | ''      |
| Expiration       | String          |         | ''      |
| Message Id       | String          |         | ''      |
| Persistent       | Boolean         |         | 'false' |
| Headers          | Key/Value pairs |         | ''      |

## Example

### Settings

```
Target Property = 'data'
Exchange Name = 'X'
Routing Key = 'XX'
Queue Name = 'XXX'
```

### Input

N/A

### Output

```
{
  "crosser": {
    "success": true
  },
  "data": {
    "body": "eyJwcmVzc3VyZSI6ODM4LCJ0ZW1wIjo4OX0=",
    "consumerTag": "amq.ctag-lGpbsAs40rVqVTxY6gXkJg",
    "deliveryTag": 1,
    "exchange": "X",
    "redelivered": false,
    "routingKey": "XX"
  }
}
```
