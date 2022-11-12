namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiossSetting
{
	[System.Text.Json.Serialization.JsonPropertyName("api_language_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLanguageType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_oss_items")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiOssItems { get; set; } = new();
}
