namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

public class ApiReqMapStartRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_deck_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiDeckId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_maparea_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiMapareaId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mapinfo_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiMapinfoNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_serial_cid")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiSerialCid { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
