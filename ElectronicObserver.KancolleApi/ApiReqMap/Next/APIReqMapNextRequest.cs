namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;

public class ApiReqMapNextRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_cell_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiCellId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_recovery_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiRecoveryType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
