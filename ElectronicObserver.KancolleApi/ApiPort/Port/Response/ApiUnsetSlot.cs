using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiPort.Port.Response;

public class ApiUnsetSlot
{

	[JsonPropertyName("api_slot_list")]
	public IEnumerable<int>? ApiSlotList { get; set; }

	[JsonPropertyName("api_type3No")]
	public int ApiType3No { get; set; }

}
