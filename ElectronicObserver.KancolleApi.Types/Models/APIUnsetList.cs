namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiUnsetList
{
	[JsonPropertyName("api_slot_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSlotList { get; set; } = new();

	[JsonPropertyName("api_type3No")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType3No { get; set; } = default!;
}
