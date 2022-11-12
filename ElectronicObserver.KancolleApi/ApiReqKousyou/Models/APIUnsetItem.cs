namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

public class ApiUnsetItem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_slot_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSlotList { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_type3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiType3 { get; set; } = default!;
}
