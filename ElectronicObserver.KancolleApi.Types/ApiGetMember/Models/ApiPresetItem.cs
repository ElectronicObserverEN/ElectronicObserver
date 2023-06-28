namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Models;

public class ApiPresetItem
{
	[JsonPropertyName("api_preset_no")]
	public int ApiPresetNo { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_selected_mode")]
	public int ApiSelectedMode { get; set; }

	[JsonPropertyName("api_lock_flag")]
	public int ApiLockFlag { get; set; }

	[JsonPropertyName("api_slot_ex_flag")]
	public int ApiSlotExFlag { get; set; }

	[JsonPropertyName("api_slot_item")]
	public List<ApiSlotItem> ApiSlotItem { get; set; } = new();

	[JsonPropertyName("api_slot_item_ex")]
	public ApiSlotItemEx? ApiSlotItemEx { get; set; }
}
