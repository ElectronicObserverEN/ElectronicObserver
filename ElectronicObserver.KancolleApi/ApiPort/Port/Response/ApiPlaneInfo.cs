using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiPort.Port.Response;

public class ApiPlaneInfo
{

	[JsonPropertyName("api_base_convert_slot")]
	public IEnumerable<int>? ApiBaseConvertSlot { get; set; }

	[JsonPropertyName("api_unset_slot")]
	public IEnumerable<ApiUnsetSlot>? ApiUnsetSlot { get; set; }

}