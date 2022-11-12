namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotDeprive;

public class ApiReqKaisouSlotDepriveRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_set_idx")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiSetIdx { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_set_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiSetShip { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_set_slot_kind")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiSetSlotKind { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_unset_idx")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiUnsetIdx { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_unset_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiUnsetShip { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_unset_slot_kind")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiUnsetSlotKind { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
