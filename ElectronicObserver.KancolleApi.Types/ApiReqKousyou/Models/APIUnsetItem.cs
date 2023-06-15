namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

public class ApiUnsetItem
{
	[JsonPropertyName("api_slot_list")]
	public List<int> ApiSlotList { get; set; } = new();

	[JsonPropertyName("api_type3")]
	public int ApiType3 { get; set; }
}
