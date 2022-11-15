namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPortPlaneInfo
{
	[System.Text.Json.Serialization.JsonPropertyName("api_base_convert_slot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiBaseConvertSlot { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_unset_slot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<ApiUnsetSlot>? ApiUnsetSlot { get; set; } = default!;
}
