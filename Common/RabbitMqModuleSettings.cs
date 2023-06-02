namespace Crosser.EdgeNode.Modules.RabbitMq.Common;

using Crosser.EdgeNode.Common.Abstractions.Utilities.Validation;
using Crosser.EdgeNode.Flows.Abstractions;

public abstract class RabbitMqModuleSettings : FlowModuleSettings
{
    [JsonSchemaExtensionData("x-sortOrder", 0)]
    [Display(Name = "Hostname", Description = "Rabbit server to connect to")]
    [DefaultValue("localhost")]
    [MinLength(1)]
    [MaxLength(256)]
    [Required]
    [NotNull]
    public string Hostname { get; set; } = "localhost";

    [JsonSchemaExtensionData("x-sortOrder", 1)]
    [Display(Name = "Port", Description = "5672 for regular (plain TCP) connections, 5671 for connections with TLS")]
    [DefaultValue(5672)]
    [Range(1, int.MaxValue)]
    public int Port { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 2)]
    [Display(Name = "Virtual Host", Description = "")]
    [DefaultValue("/")]
    [MinLength(0)]
    [MaxLength(64)]
    [Required]
    [NotNull]
    public string VirtualHost { get; set; } = "/";

    [JsonSchemaExtensionData("x-sortOrder", 3)]
    [Display(Name = "Use TLS", Description = "Check to enable Transport Layer Security. You might need to change the port when enabled (default is 5671 for TLS)")]
    [DefaultValue(false)]
    public bool UseTLS { get; set; } = false;

    [JsonSchemaExtensionData("x-sortOrder", 4)]
    [Display(Name = "Credential", Description = "")]
    [JsonSchemaExtensionData(Flows.Abstractions.Credential.ATTRIBUTE, Flows.Abstractions.Credential.Types.UsernameAndPassword)]
    public Guid? Credential { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 8)]
    [Display(Name = "Exchange Settings", Description = "")]
    public ExchangeSettings ExchangeSettings { get; set; } = new();

    [JsonSchemaExtensionData("x-sortOrder", 9)]
    [Display(Name = "Channel Basic Settings", Description = "")]
    public ChannelBasicProperties ChannelBasicProperties { get; set; } = new();

    public override void Validate(SettingsValidator validator)
    {
        validator.Validate(nameof(this.Hostname), this.Hostname).NotNull().MinLength(1).MaxLength(256);
        validator.Validate(nameof(this.Port), this.Port).MinValue(1).MaxValue(int.MaxValue);
        validator.Validate(nameof(this.VirtualHost), this.VirtualHost).NotNull().MinLength(0).MaxLength(64);

        this.ExchangeSettings.Validate(validator);

        base.Validate(validator);
    }
}
