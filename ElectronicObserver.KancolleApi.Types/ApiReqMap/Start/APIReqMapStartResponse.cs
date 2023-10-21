using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

public class ApiReqMapStartResponse : IMapProgressApi
{
	[JsonPropertyName("api_airsearch")]
	public ApiAirsearch ApiAirsearch { get; set; } = new();

	[JsonPropertyName("api_bosscell_no")]
	public int ApiBosscellNo { get; set; }

	[JsonPropertyName("api_bosscomp")]
	public int ApiBosscomp { get; set; }

	[JsonPropertyName("api_cell_data")]
	public List<ApiCellDatum> ApiCellData { get; set; } = new();

	[JsonPropertyName("api_cell_flavor")]
	public ApiCellFlavor? ApiCellFlavor { get; set; }

	[JsonPropertyName("api_color_no")]
	public int ApiColorNo { get; set; }

	/// <summary>
	/// Enemy fleet preview. Only one element against single fleet. Two elements if fighting combined fleet.
	/// </summary>
	[JsonPropertyName("api_e_deck_info")]
	public List<EDeckInfo>? ApiEDeckInfo { get; set; }

	[JsonPropertyName("api_event_id")]
	public int ApiEventId { get; set; }

	[JsonPropertyName("api_event_kind")]
	public int ApiEventKind { get; set; }

	[JsonPropertyName("api_eventmap")]
	public ApiEventmap? ApiEventmap { get; set; }

	[JsonPropertyName("api_from_no")]
	public int ApiFromNo { get; set; }

	[JsonPropertyName("api_happening")]
	public ApiHappening? ApiHappening { get; set; }

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
	public ApiSelectRoute? ApiSelectRoute { get; set; }
}
