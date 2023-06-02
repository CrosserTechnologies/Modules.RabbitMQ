namespace Crosser.EdgeNode.Modules.RabbitMq.Consumer;

public class RabbitMqConsumerModuleSettings : RabbitMqModuleSettings
{
    [JsonSchemaExtensionData("x-sortOrder", 0)]
    [Display(Name = "Target Property", Description = "The property that will contain the message")]
    [DefaultValue("data")]
    [MinLength(1)]
    [MaxLength(64)]
    [Required]
    [NotNull]
    public string TargetProperty { get; set; } = "data";

    [JsonSchemaExtensionData("x-sortOrder", 5)]
    [Display(Name = "Basic QoS", Description = "")]
    public BasicQoS BasicQoS { get; set; } = new();

    [JsonSchemaExtensionData("x-sortOrder", 6)]
    [Display(Name = "Auto Acknowledge", Description = "")]
    [DefaultValue(true)]
    public bool AutoAcknowledge { get; set; } = true;

    [JsonSchemaExtensionData("x-sortOrder", 7)]
    [Display(Name = "Queue Settings", Description = "")]
    public QueueSettings QueueSettings { get; set; } = new();

    public override void Validate(SettingsValidator validator)
    {
        validator.Validate(nameof(this.TargetProperty), this.TargetProperty).NotNull().MinLength(1).MaxLength(64);
        validator.Validate(nameof(this.QueueSettings.QueueName), this.QueueSettings.QueueName).NotNull().MinLength(1).MaxLength(256);

        validator.Validate(nameof(this.BasicQoS.PrefetchCount), this.BasicQoS.PrefetchCount).MinValue(ushort.MinValue).MaxValue(ushort.MaxValue);
        validator.Validate(nameof(this.BasicQoS.PrefetchSize), this.BasicQoS.PrefetchSize).MinValue(uint.MinValue).MaxValue(uint.MaxValue);
        base.Validate(validator);
    }
}
