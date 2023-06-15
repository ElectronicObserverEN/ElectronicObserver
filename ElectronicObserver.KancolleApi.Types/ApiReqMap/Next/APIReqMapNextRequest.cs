namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;

public class ApiReqMapNextRequest
{
	[JsonPropertyName("api_cell_id")]
	public string? ApiCellId { get; set; }

	[JsonPropertyName("api_recovery_type")]
	public string ApiRecoveryType { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
