namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiUnsetList
{
	[System.Text.Json.Serialization.JsonPropertyName("api_slot_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSlotList { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_type3No")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiType3No { get; set; } = default!;
}
