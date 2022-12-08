using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Models;

public class ApiPresetItem
{
	[JsonPropertyName("api_preset_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiPresetNo { get; set; }

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_selected_mode")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiSelectedMode { get; set; }

	[JsonPropertyName("api_lock_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiLockFlag { get; set; }

	[JsonPropertyName("api_slot_ex_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiSlotExFlag { get; set; }

	[JsonPropertyName("api_slot_item")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiSlotItem> ApiSlotItem { get; set; } = default!;

	[JsonPropertyName("api_slot_item_ex")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiSlotItemEx ApiSlotItemEx { get; set; } = default!;
}
