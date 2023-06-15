namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Supply;

public class ApiReqAirCorpsSupplyRequest
{
	[JsonPropertyName("api_area_id")]
	public string ApiAreaId { get; set; } = default!;

	[JsonPropertyName("api_base_id")]
	public string ApiBaseId { get; set; } = default!;

	[JsonPropertyName("api_squadron_id")]
	public string ApiSquadronId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
