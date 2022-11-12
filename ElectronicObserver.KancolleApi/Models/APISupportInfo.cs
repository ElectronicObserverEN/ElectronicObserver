namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportInfo
{
	[System.Text.Json.Serialization.JsonPropertyName("api_support_airatack")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiSupportAiratack? ApiSupportAiratack { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_support_hourai")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiSupportHourai ApiSupportHourai { get; set; } = new();
}
