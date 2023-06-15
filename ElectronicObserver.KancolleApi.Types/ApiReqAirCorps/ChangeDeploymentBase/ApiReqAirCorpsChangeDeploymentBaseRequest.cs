namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ChangeDeploymentBase;

public class ApiReqAirCorpsChangeDeploymentBaseRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_area_id")]
	public string ApiAreaId { get; set; } = "";

	[JsonPropertyName("api_base_id")]
	public string ApiBaseId { get; set; } = "";

	[JsonPropertyName("api_squadron_id")]
	public string ApiSquadronId { get; set; } = "";

	[JsonPropertyName("api_item_id")]
	public string ApiItemId { get; set; } = "";

	[JsonPropertyName("api_base_id_src")]
	public string ApiBaseIdSrc { get; set; } = "";
}
