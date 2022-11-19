using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetSlot;

public class ApiGetMemberPresetSlotResponse
{
	[JsonPropertyName("api_max_num")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiMaxNum { get; set; }

	[JsonPropertyName("api_preset_items")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiPresetItem> ApiPresetItems { get; set; } = new();
}
