namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

public class ApiReqMapStartRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = "";

	[JsonPropertyName("api_maparea_id")]
	public string ApiMapareaId { get; set; } = "";

	[JsonPropertyName("api_mapinfo_no")]
	public string ApiMapinfoNo { get; set; } = "";

	[JsonPropertyName("api_serial_cid")]
	public string ApiSerialCid { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
