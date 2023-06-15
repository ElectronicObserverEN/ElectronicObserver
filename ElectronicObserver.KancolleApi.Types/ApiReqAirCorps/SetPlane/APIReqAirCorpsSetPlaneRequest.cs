namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetPlane;

public class ApiReqAirCorpsSetPlaneRequest
{
	[JsonPropertyName("api_area_id")]
	public string ApiAreaId { get; set; } = "";

	[JsonPropertyName("api_base_id")]
	public string ApiBaseId { get; set; } = "";

	[JsonPropertyName("api_item_id")]
	public string ApiItemId { get; set; } = "";

	[JsonPropertyName("api_squadron_id")]
	public string ApiSquadronId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
