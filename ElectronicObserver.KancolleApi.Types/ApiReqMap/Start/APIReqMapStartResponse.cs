using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

public class ApiReqMapStartResponse : IMapProgressApi
{
	[JsonPropertyName("api_airsearch")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiAirsearch ApiAirsearch { get; set; } = new();

	[JsonPropertyName("api_bosscell_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBosscellNo { get; set; } = default!;

	[JsonPropertyName("api_bosscomp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBosscomp { get; set; } = default!;

	[JsonPropertyName("api_cell_data")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiCellDatum> ApiCellData { get; set; } = new();

	[JsonPropertyName("api_cell_flavor")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiCellFlavor? ApiCellFlavor { get; set; } = default!;

	[JsonPropertyName("api_color_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiColorNo { get; set; } = default!;

	/// <summary>
	/// Enemy fleet preview. Only one element against single fleet. Two elements if fighting combined fleet.
	/// </summary>
	[JsonPropertyName("api_e_deck_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<EDeckInfo>? ApiEDeckInfo { get; set; } = default!;

	[JsonPropertyName("api_event_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEventId { get; set; } = default!;

	[JsonPropertyName("api_event_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEventKind { get; set; } = default!;

	[JsonPropertyName("api_eventmap")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiEventmap? ApiEventmap { get; set; } = default!;

	[JsonPropertyName("api_from_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFromNo { get; set; } = default!;

	[JsonPropertyName("api_happening")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHappening? ApiHappening { get; set; } = default!;

	[JsonPropertyName("api_maparea_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_mapinfo_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMapinfoNo { get; set; } = default!;

	[JsonPropertyName("api_next")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNext { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_rashin_flg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRashinFlg { get; set; } = default!;

	[JsonPropertyName("api_rashin_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRashinId { get; set; } = default!;

	[JsonPropertyName("api_select_route")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiSelectRoute? ApiSelectRoute { get; set; } = default!;
}
