﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ChangeDeploymentBase;

public class ApiReqAirCorpsChangeDeploymentBaseRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_area_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiAreaId { get; set; } = default!;

	[JsonPropertyName("api_base_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiBaseId { get; set; } = default!;

	[JsonPropertyName("api_squadron_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSquadronId { get; set; } = default!;

	[JsonPropertyName("api_item_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiItemId { get; set; } = default!;

	[JsonPropertyName("api_base_id_src")]
	[Required(AllowEmptyStrings = true)]
	public string ApiBaseIdSrc { get; set; } = default!;
}
