namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiossSetting
{
	[JsonPropertyName("api_language_type")]
	public int ApiLanguageType { get; set; }

	[JsonPropertyName("api_oss_items")]
	public List<int> ApiOssItems { get; set; } = new();
}
