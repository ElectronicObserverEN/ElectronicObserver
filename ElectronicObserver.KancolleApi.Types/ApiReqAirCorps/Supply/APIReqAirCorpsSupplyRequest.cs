namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Supply;

public class ApiReqAirCorpsSupplyRequest
{
	[JsonPropertyName("api_area_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiAreaId { get; set; } = default!;

	[JsonPropertyName("api_base_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiBaseId { get; set; } = default!;

	[JsonPropertyName("api_squadron_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiSquadronId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
