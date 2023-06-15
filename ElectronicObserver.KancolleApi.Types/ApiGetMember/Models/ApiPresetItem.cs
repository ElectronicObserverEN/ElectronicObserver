using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Models;

public class ApiPresetItem
{
	[JsonPropertyName("api_preset_no")]
	[Required]
	public int ApiPresetNo { get; set; }

	[JsonPropertyName("api_name")]
	[Required]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_selected_mode")]
	[Required]
	public int ApiSelectedMode { get; set; }

	[JsonPropertyName("api_lock_flag")]
	[Required]
	public int ApiLockFlag { get; set; }

	[JsonPropertyName("api_slot_ex_flag")]
	[Required]
	public int ApiSlotExFlag { get; set; }

	[JsonPropertyName("api_slot_item")]
	[Required]
	public List<ApiSlotItem> ApiSlotItem { get; set; } = default!;

	[JsonPropertyName("api_slot_item_ex")]
	[Required]
	public ApiSlotItemEx ApiSlotItemEx { get; set; } = default!;
}
