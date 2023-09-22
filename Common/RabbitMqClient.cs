namespace Crosser.EdgeNode.Modules.RabbitMq.Common;

public partial class RabbitMqClient : IDisposable
{
    public IConnection? Connection { get; private set; }
    public IModel? Channel { get; private set; }
    public EventHandler<BasicDeliverEventArgs>? OnMessageReceived { get; set; }
    public EventHandler? OnDisconnected { get; set; }
    protected IBasicProperties? ChannelProperties;
    protected readonly ConnectionFactory factory;
    protected readonly RabbitMqModuleSettings Settings;

    public RabbitMqClient(RabbitMqModuleSettings settings)
    {
        this.Settings = settings;
        this.factory = new()
        {
            VirtualHost = this.Settings.VirtualHost,
            HostName = this.Settings.Hostname
        };
        if (this.Settings.Credential.HasValue)
        {
            var credential = this.Settings.GetCredentialById(this.Settings.Credential.Value);
            if (credential.Data is not null && credential.Data.Length > 0)
            {
                var userNameAndPassword = credential.ToCredential<CredentialWithUsernamePassword>();
                this.factory.UserName = userNameAndPassword.Username;
                this.factory.Password = userNameAndPassword.Password;
            }
            else
            {
                var userNameAndPassword = (CredentialWithUsernamePassword)credential;
                this.factory.UserName = userNameAndPassword.Username;
                this.factory.Password = userNameAndPassword.Password;
            }
        }
        if (this.Settings.UseTLS)
        {
            this.factory.Ssl = new SslOption
            {
                Enabled = true,
                ServerName = this.Settings.Hostname
            };
        }
    }

    public IResult<IConnection> GetConnection()
    {
        try
        {
            if (this.Connection is not null && this.Connection.IsOpen)
            {
                return Result<IConnection>.Ok(this.Connection);
            }

            this.Connection = factory.CreateConnection();

            this.Connection.ConnectionShutdown += (object? _, ShutdownEventArgs e) =>
            {
                this.OnDisconnected?.Invoke(this, EventArgs.Empty);
            };

            return Result<IConnection>.Ok(this.Connection);
        }
        catch (Exception ex)
        {
            return Result<IConnection>.Fault($"Failed to create connection to RabbitMQ: {ex.Message}");
        }
    }

    public IResult<IModel> GetChannel()
    {
        try
        {
            if (this.Connection is null || this.Connection.IsOpen is false)
            {
                var connectResult = this.GetConnection();
                if (connectResult.IsError)
                {
                    return Result<IModel>.Fault(connectResult.Error.Message);
                }
            }

            if (this.Connection is null)
            {
                return Result<IModel>.Fault("Connection could not be created");
            }

            if (this.Channel is not null)
            {
                return Result<IModel>.Ok(this.Channel);
            }

            this.Channel = this.Connection.CreateModel();

            this.Channel.ExchangeDeclare(this.Settings.ExchangeSettings.ExchangeName,
                                         this.Settings.ExchangeSettings.ExchangeTypeValue.ToString().ToLower(),
                                         this.Settings.ExchangeSettings.DurableExchange,
                                         this.Settings.ExchangeSettings.AutoDeleteExchange,
                                         this.Settings.ExchangeSettings.ExchangeDeclarationProperties);

            this.ChannelProperties = this.Channel.CreateBasicProperties();
            this.ChannelProperties.AppId = this.Settings.ChannelBasicProperties.AppId;
            this.ChannelProperties.ContentEncoding = this.Settings.ChannelBasicProperties.ContentEncoding;
            this.ChannelProperties.ContentType = this.Settings.ChannelBasicProperties.ContentType;
            this.ChannelProperties.CorrelationId = this.Settings.ChannelBasicProperties.CorrelationId;
            this.ChannelProperties.Expiration = this.Settings.ChannelBasicProperties.Expiration;
            this.ChannelProperties.MessageId = this.Settings.ChannelBasicProperties.MessageId;
            this.ChannelProperties.Persistent = this.Settings.ChannelBasicProperties.Persistent;
            this.ChannelProperties.Headers = this.Settings.ChannelBasicProperties.Headers;

            return Result<IModel>.Ok(this.Channel);
        }
        catch (Exception ex)
        {
            return Result<IModel>.Fault($"Failed to create channel to RabbitMQ: {ex.Message}");
        }
    }



    private bool _disposedValue;
    private bool _disposeCalled;

    public bool ShutDownInitiated => this._disposeCalled;

    protected virtual void Dispose(bool disposing)
    {
        this._disposeCalled = true;
        if (!this._disposedValue)
        {
            if (disposing)
            {
                this.Channel?.Close();
                this.Connection?.Close();
            }
            this._disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}