namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPortPlaneInfo
{
	[JsonPropertyName("api_base_convert_slot")]
	public List<int>? ApiBaseConvertSlot { get; set; }

	[JsonPropertyName("api_unset_slot")]
	public List<ApiUnsetSlot>? ApiUnsetSlot { get; set; }
}
