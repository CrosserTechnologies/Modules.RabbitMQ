namespace Crosser.EdgeNode.Modules.RabbitMq.Consumer;

public class QueueSettings
{
    [JsonSchemaExtensionData("x-sortOrder", 0)]
    [Display(Name = "Name", Description = "Name of the queue to use or create.")]
    [DefaultValue("")]
    [MinLength(1)]
    [MaxLength(256)]
    [Required]
    [NotNull]
    public string QueueName { get; set; } = "";

    [JsonSchemaExtensionData("x-sortOrder", 1)]
    [Display(Name = "Durable", Description = "Creates a durable queue when selected.")]
    [DefaultValue(false)]
    public bool DurableQueue { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 2)]
    [Display(Name = "Exclusive", Description = "Creates an exclusive queue when selected.")]
    [DefaultValue(false)]
    public bool ExclusiveQueue { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 3)]
    [Display(Name = "Auto Delete", Description = "Automatically deletes a queue after all consumers unsubscribe.")]
    [DefaultValue(false)]
    public bool AutoDeleteQueue { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 4)]
    [Display(Name = "Declaration Properties", Description = "Additional queue declaration properties to use.")]
    [JsonSchemaExtensionData("x-keyDescription", "Key")]
    [JsonSchemaExtensionData("x-valueDescription", "Value")]
    public Dictionary<string, object> QueueDeclarationProperties { get; set; } = new();
}
