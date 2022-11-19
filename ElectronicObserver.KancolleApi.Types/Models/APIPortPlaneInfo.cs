namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPortPlaneInfo
{
	[JsonPropertyName("api_base_convert_slot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiBaseConvertSlot { get; set; } = default!;

	[JsonPropertyName("api_unset_slot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApiUnsetSlot>? ApiUnsetSlot { get; set; } = default!;
}
