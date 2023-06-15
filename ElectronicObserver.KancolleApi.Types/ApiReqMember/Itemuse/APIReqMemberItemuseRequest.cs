namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Itemuse;

public class ApiReqMemberItemuseRequest
{
	[JsonPropertyName("api_exchange_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiExchangeType { get; set; } = default!;

	[JsonPropertyName("api_force_flag")]
	[Required(AllowEmptyStrings = true)]
	public string ApiForceFlag { get; set; } = default!;

	[JsonPropertyName("api_useitem_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiUseitemId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
