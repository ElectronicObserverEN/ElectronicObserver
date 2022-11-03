using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiReqMap.Start.Response;

public class ApiReqMapStartResponse
{

	[JsonPropertyName("api_airsearch")]
	public ApiAirsearch ApiAirsearch { get; set; }

	[JsonPropertyName("api_bosscell_no")]
	public int ApiBosscellNo { get; set; }

	[JsonPropertyName("api_bosscomp")]
	public int ApiBosscomp { get; set; }

	[JsonPropertyName("api_cell_data")]
	public IEnumerable<ApiCellDatum> ApiCellData { get; set; }

	[JsonPropertyName("api_cell_flavor")]
	public ApiCellFlavor ApiCellFlavor { get; set; }

	[JsonPropertyName("api_color_no")]
	public int ApiColorNo { get; set; }

	[JsonPropertyName("api_event_id")]
	public int ApiEventId { get; set; }

	[JsonPropertyName("api_event_kind")]
	public int ApiEventKind { get; set; }

	[JsonPropertyName("api_eventmap")]
	public ApiEventmap ApiEventmap { get; set; }

	[JsonPropertyName("api_from_no")]
	public int ApiFromNo { get; set; }

	[JsonPropertyName("api_happening")]
	public ApiHappening ApiHappening { get; set; }

	[JsonPropertyName("api_maparea_id")]
	public int ApiMapareaId { get; set; }

	[JsonPropertyName("api_mapinfo_no")]
	public int ApiMapinfoNo { get; set; }

	[JsonPropertyName("api_next")]
	public int ApiNext { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_rashin_flg")]
	public int ApiRashinFlg { get; set; }

	[JsonPropertyName("api_rashin_id")]
	public int ApiRashinId { get; set; }

	[JsonPropertyName("api_select_route")]
	public ApiSelectRoute ApiSelectRoute { get; set; }

}
