namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Itemuse;

public class ApiReqMemberItemuseRequest
{
	[JsonPropertyName("api_exchange_type")]
	public string ApiExchangeType { get; set; } = default!;

	[JsonPropertyName("api_force_flag")]
	public string ApiForceFlag { get; set; } = default!;

	[JsonPropertyName("api_useitem_id")]
	public string ApiUseitemId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
