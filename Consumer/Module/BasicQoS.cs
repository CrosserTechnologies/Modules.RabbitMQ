namespace Crosser.EdgeNode.Modules.RabbitMq.Consumer;

public class BasicQoS
{
    [JsonSchemaExtensionData("x-sortOrder", 1)]
    [Display(Name = "Prefetch Size", Description = "")]
    [DefaultValue(0)]
    [Range(uint.MinValue, uint.MaxValue)]
    public uint PrefetchSize { get; set; } = 0;

    [JsonSchemaExtensionData("x-sortOrder", 2)]
    [Display(Name = "Prefetch Count", Description = "")]
    [DefaultValue(0)]
    [Range(ushort.MinValue, ushort.MaxValue)]
    public ushort PrefetchCount { get; set; } = 0;

    [JsonSchemaExtensionData("x-sortOrder", 3)]
    [Display(Name = "Global", Description = "")]
    [DefaultValue(false)]
    public bool Global { get; set; } = false;
}
