namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiossSetting
{
	[JsonPropertyName("api_language_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLanguageType { get; set; } = default!;

	[JsonPropertyName("api_oss_items")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiOssItems { get; set; } = new();
}
