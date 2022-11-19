namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.CanPresetSlotSelect;

public class APIReqKaisouCanPresetSlotSelectResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiFlag { get; set; } = default!;
}
