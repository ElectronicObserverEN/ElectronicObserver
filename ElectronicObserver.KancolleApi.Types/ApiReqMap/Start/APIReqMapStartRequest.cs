namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

public class ApiReqMapStartRequest
{
	[JsonPropertyName("api_deck_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_maparea_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_mapinfo_no")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMapinfoNo { get; set; } = default!;

	[JsonPropertyName("api_serial_cid")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSerialCid { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
