namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

public class ApiUnsetItem
{
	[JsonPropertyName("api_slot_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSlotList { get; set; } = new();

	[JsonPropertyName("api_type3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType3 { get; set; } = default!;
}
