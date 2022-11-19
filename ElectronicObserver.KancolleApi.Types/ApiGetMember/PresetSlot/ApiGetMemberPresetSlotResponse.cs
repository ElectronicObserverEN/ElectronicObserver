using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetSlot;

public class ApiGetMemberPresetSlotResponse
{
	[JsonPropertyName("api_max_num")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiMaxNum { get; set; }

	[JsonPropertyName("api_preset_items")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiPresetItem> ApiPresetItems { get; set; } = new();
}
