namespace Crosser.EdgeNode.Modules.RabbitMq.Common;

public class ExchangeSettings
{

    [JsonSchemaExtensionData("x-sortOrder", 0)]
    [Display(Name = "Name", Description = "Binding name.")]
    [DefaultValue("")]
    [MinLength(1)]
    [MaxLength(256)]
    [Required]
    [NotNull]
    public string ExchangeName { get; set; } = "";

    [JsonSchemaExtensionData("x-sortOrder", 1)]
    [Display(Name = "Type", Description = "Binding type.")]
    [DefaultValue(ExchangeTypeValue.Direct)]
    public ExchangeTypeValue ExchangeTypeValue { get; set; } = ExchangeTypeValue.Direct;

    [JsonSchemaExtensionData("x-sortOrder", 2)]
    [Display(Name = "Durable", Description = "Creates a durable exchange.")]
    [DefaultValue(false)]
    public bool DurableExchange { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 3)]
    [Display(Name = "Auto Delete", Description = "Automatically deletes an exchange when all queues are finished using it.")]
    [DefaultValue(false)]
    public bool AutoDeleteExchange { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 4)]
    [Display(Name = "Routing Key", Description = "Leave empty to default to the queue name.")]
    [MinLength(0)]
    [MaxLength(256)]
    [Required]
    [NotNull]
    public string? ExchangeRoutingKey { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 5)]
    [Display(Name = "Declaration Properties", Description = "Additional exchange declaration properties to use.")]
    [JsonSchemaExtensionData("x-keyDescription", "Key")]
    [JsonSchemaExtensionData("x-valueDescription", "Value")]
    public Dictionary<string, object> ExchangeDeclarationProperties { get; set; } = new();

    [JsonSchemaExtensionData("x-sortOrder", 6)]
    [Display(Name = "Binding Properties", Description = "Additional exchange binding properties to use.")]
    [JsonSchemaExtensionData("x-keyDescription", "Key")]
    [JsonSchemaExtensionData("x-valueDescription", "Value")]
    public Dictionary<string, object> ExchangeBindingProperties { get; set; } = new();

    public void Validate(SettingsValidator validator)
    {
        validator.Validate(nameof(this.ExchangeName), this.ExchangeName).NotNull().MinLength(1).MaxLength(256);
        validator.Validate(nameof(this.ExchangeRoutingKey), this.ExchangeRoutingKey).NotNull().MinLength(0).MaxLength(256);
    }
}
