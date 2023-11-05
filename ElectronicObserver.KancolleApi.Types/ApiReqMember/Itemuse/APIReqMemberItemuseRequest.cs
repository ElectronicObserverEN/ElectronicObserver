namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Itemuse;

public class ApiReqMemberItemuseRequest
{
	[JsonPropertyName("api_exchange_type")]
	public string ApiExchangeType { get; set; } = "";

	[JsonPropertyName("api_force_flag")]
	public string ApiForceFlag { get; set; } = "";

	[JsonPropertyName("api_useitem_id")]
	public string ApiUseitemId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
