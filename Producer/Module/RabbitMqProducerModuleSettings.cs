namespace Crosser.EdgeNode.Modules.RabbitMq.Producer;

public class RabbitMqProducerModuleSettings : RabbitMqModuleSettings
{
    [JsonSchemaExtensionData("x-sortOrder", 0)]
    [Display(Name = "Source Property", Description = "The property that contains the message to send")]
    [DefaultValue("data")]
    [MinLength(1)]
    [MaxLength(64)]
    [Required]
    [NotNull]
    public string SourceProperty { get; set; } = "data";

    public override void Validate(SettingsValidator validator)
    {
        validator.Validate(nameof(this.SourceProperty), this.SourceProperty).NotNull().MinLength(1).MaxLength(64);
        base.Validate(validator);
    }
}
