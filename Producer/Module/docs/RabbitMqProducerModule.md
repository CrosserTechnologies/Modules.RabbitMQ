# RabbitMQ Producer

Will send messages to RabbitMQ

## Settings

| Name                   | Requirements                       | Purpose                                                                                                            | Default     |
| ---------------------- | ---------------------------------- | ------------------------------------------------------------------------------------------------------------------ | ----------- |
| Source Property        | String with length `1 to 64`       | The property that contains the message to send                                                                     | 'data'      |
| Hostname               | String with length `1 to 256`      | Rabbit server to connect to                                                                                        | 'localhost' |
| Port                   | Number larger than 0               | Port of the RabbitMq server. Normally 5672 for regular (plain TCP) connections, 5671 for connections with TLS      | '5672'      |
| Virtual Host           | String with length `0 to 64`       |                                                                                                                    | '/'         |
| Use TLS                | Boolean                            | Check to enable Transport Layer Security. You might need to change the port when enabled (default is 5671 for TLS) | 'false'     |
| Credential             | Username & Password                |                                                                                                                    | 'false'     |
| Exchange Settings      | See Exchange Settings section      |                                                                                                                    |             |
| Channel Basic Settings | See Channel Basic Settings section |                                                                                                                    |             |

### Exchange Settings

| Name                   | Requirements                            | Purpose                                                                                                                                     | Default  |
| ---------------------- | --------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------- | -------- |
| Name                   | String with length `1 to 256`           | Binding name.                                                                                                                               | ''       |
| Type                   | One of `Direct, Fanout, Headers, Topic` | Binding type.                                                                                                                               | 'Direct' |
| Durable                | Boolean                                 | Creates a durable exchange when selected.                                                                                                   | 'false'  |
| Auto Delete            | Boolean                                 | Automatically deletes an exchange when all queues are finished using it.                                                                    | 'false'  |
| Routing Key            | String with length `0 to 256`           | Leave empty to default to the queue name. Template syntax is also available to get the routing key from the incoming message (see example). | ''       |
| Declaration Properties | Key/Value pairs                         | Additional exchange declaration properties to use.                                                                                          | ''       |
| Binding Properties     | Key/Value pairs                         | Additional exchange binding properties to use.                                                                                              | ''       |

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

## Example 1

### Settings

```
Source Property = 'data'
```

### Input

If the source property targets a complex structure like `data` in this example the module will transform `data` into JSON and send the JSON representation as bytes.
If the source property targets a simple value like a number, that value will be converted into a string and then sent as bytes.

```
{
  "data": {
    "pressure": 838,
    "temp": 89
  },
  "id": 1,
  "name": "machine"
}
```

### Output

```
{
  "crosser": {
    "success": true
  },
  "data": {
    "pressure": 838,
    "temp": 89
  },
  "id": 1,
  "name": "machine"
}
```

## Example 2

Using `template syntax` to get the routing key `dynamically` from the incoming message.

### Settings

```
Source Property = 'data'
Exchange Settings > Routing Key = '{routing.key}'
```

### Input

If the source property targets a complex structure like `data` in this example the module will transform `data` into JSON and send the JSON representation as bytes.
If the source property targets a simple value like a number, that value will be converted into a string and then sent as bytes.

```
{
  "data": {
    "pressure": 838,
    "temp": 89
  },
  "id": 1,
  "name": "machine",
  "routing" : {
    "key": "my-routing-key"
  }
}
```

### Output

```
{
  "crosser": {
    "success": true
  },
  "data": {
    "pressure": 838,
    "temp": 89
  },
  "id": 1,
  "name": "machine",
  "routing" : {
    "key": "my-routing-key"
  }
}
```
