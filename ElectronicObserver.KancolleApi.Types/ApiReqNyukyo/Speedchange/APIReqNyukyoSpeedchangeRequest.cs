namespace ElectronicObserver.KancolleApi.Types.ApiReqNyukyo.Speedchange;

public class ApiReqNyukyoSpeedchangeRequest
{
	[JsonPropertyName("api_ndock_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNdockId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
