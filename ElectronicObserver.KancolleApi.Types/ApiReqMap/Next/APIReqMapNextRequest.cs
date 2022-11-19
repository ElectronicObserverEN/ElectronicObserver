namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;

public class ApiReqMapNextRequest
{
	[JsonPropertyName("api_cell_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiCellId { get; set; } = default!;

	[JsonPropertyName("api_recovery_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiRecoveryType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
