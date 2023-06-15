using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;

public class ApiReqMapNextResponse : IMapProgressApi
{
	[JsonPropertyName("api_airsearch")]
	public ApiAirsearch ApiAirsearch { get; set; } = new();

	[JsonPropertyName("api_bosscell_no")]
	public int ApiBosscellNo { get; set; }

	[JsonPropertyName("api_bosscomp")]
	public int ApiBosscomp { get; set; }

	[JsonPropertyName("api_cell_flavor")]
	public ApiCellFlavor? ApiCellFlavor { get; set; }

	[JsonPropertyName("api_color_no")]
	public int ApiColorNo { get; set; }

	[JsonPropertyName("api_comment_kind")]
	public int ApiCommentKind { get; set; }

	/// <summary>
	/// Enemy fleet preview. Only one element against single fleet. Two elements if fighting combined fleet.
	/// </summary>
	[JsonPropertyName("api_e_deck_info")]
	public List<EDeckInfo>? ApiEDeckInfo { get; set; }

	[JsonPropertyName("api_destruction_battle")]
	public ApiDestructionBattle? ApiDestructionBattle { get; set; }

	[JsonPropertyName("api_event_id")]
	public int ApiEventId { get; set; }

	[JsonPropertyName("api_event_kind")]
	public int ApiEventKind { get; set; }

	[JsonPropertyName("api_eventmap")]
	public ApiEventmap? ApiEventmap { get; set; }

	[JsonPropertyName("api_get_eo_rate")]
	public int? ApiGetEoRate { get; set; }

	[JsonPropertyName("api_happening")]
	public ApiHappening? ApiHappening { get; set; }

	/// <summary>
	/// Element type is <see cref="Models.ApiItemget"/> or <see cref="List{T}"/> of <see cref="Models.ApiItemget"/>s.
	/// </summary>
	[JsonPropertyName("api_itemget")]
	public object? ApiItemget { get; set; }

	[JsonPropertyName("api_itemget_eo_comment")]
	public ApiItemgetEo? ApiItemgetEoComment { get; set; }

	[JsonPropertyName("api_itemget_eo_result")]
	public ApiItemgetEo? ApiItemgetEoResult { get; set; }

	[JsonPropertyName("api_maparea_id")]
	public int ApiMapareaId { get; set; }

	[JsonPropertyName("api_mapinfo_no")]
	public int ApiMapinfoNo { get; set; }

	[JsonPropertyName("api_next")]
	public int ApiNext { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_offshore_supply")]
	public ApiOffshoreSupply? ApiOffshoreSupply { get; set; }

	[JsonPropertyName("api_production_kind")]
	public int ApiProductionKind { get; set; }

	[JsonPropertyName("api_rashin_flg")]
	public int ApiRashinFlg { get; set; }

	[JsonPropertyName("api_rashin_id")]
	public int ApiRashinId { get; set; }

	[JsonPropertyName("api_ration_flag")]
	public int? ApiRationFlag { get; set; }

	[JsonPropertyName("api_select_route")]
	public ApiSelectRoute? ApiSelectRoute { get; set; }
}
