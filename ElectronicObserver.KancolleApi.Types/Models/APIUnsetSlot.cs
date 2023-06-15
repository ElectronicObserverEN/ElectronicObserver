namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiUnsetSlot
{
	[JsonPropertyName("api_slot_list")]
	public List<int> ApiSlotList { get; set; } = new();

	[JsonPropertyName("api_type3No")]
	public int ApiType3No { get; set; }
}
