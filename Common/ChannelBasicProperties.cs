namespace Crosser.EdgeNode.Modules.RabbitMq.Common;

public class ChannelBasicProperties
{
    [JsonSchemaExtensionData("x-sortOrder", 0)]
    [Display(Name = "AppId", Description = "")]
    public string? AppId { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 2)]
    [Display(Name = "Content Encoding", Description = "")]
    public string? ContentEncoding { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 3)]
    [Display(Name = "Content Type", Description = "")]
    public string? ContentType { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 4)]
    [Display(Name = "Correlation Id", Description = "")]
    public string? CorrelationId { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 5)]
    [Display(Name = "Expiration", Description = "")]
    public string? Expiration { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 6)]
    [Display(Name = "Message Id", Description = "")]
    public string? MessageId { get; set; }

    [JsonSchemaExtensionData("x-sortOrder", 7)]
    [Display(Name = "Persistent", Description = "")]
    [DefaultValue(false)]
    public bool Persistent { get; set; } = false;

    [JsonSchemaExtensionData("x-sortOrder", 8)]
    [Display(Name = "Headers", Description = "")]
    [JsonSchemaExtensionData("x-keyDescription", "Key")]
    [JsonSchemaExtensionData("x-valueDescription", "Value")]
    public Dictionary<string, object> Headers { get; set; } = new();
}
