using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;

public class ApiReqMapNextResponse : IMapProgressApi
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

	[JsonPropertyName("api_cell_flavor")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiCellFlavor? ApiCellFlavor { get; set; } = default!;

	[JsonPropertyName("api_color_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiColorNo { get; set; } = default!;

	[JsonPropertyName("api_comment_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCommentKind { get; set; } = default!;

	/// <summary>
	/// Enemy fleet preview. Only one element against single fleet. Two elements if fighting combined fleet.
	/// </summary>
	[JsonPropertyName("api_e_deck_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<EDeckInfo>? ApiEDeckInfo { get; set; } = default!;

	[JsonPropertyName("api_destruction_battle")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiDestructionBattle? ApiDestructionBattle { get; set; } = default!;

	[JsonPropertyName("api_event_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEventId { get; set; } = default!;

	[JsonPropertyName("api_event_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEventKind { get; set; } = default!;

	[JsonPropertyName("api_eventmap")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiEventmap? ApiEventmap { get; set; } = default!;

	[JsonPropertyName("api_get_eo_rate")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiGetEoRate { get; set; } = default!;

	[JsonPropertyName("api_happening")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHappening? ApiHappening { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="Models.ApiItemget"/> or <see cref="List{T}"/> of <see cref="Models.ApiItemget"/>s.
	/// </summary>
	[JsonPropertyName("api_itemget")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object? ApiItemget { get; set; } = default!;

	[JsonPropertyName("api_itemget_eo_comment")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiItemgetEo? ApiItemgetEoComment { get; set; } = default!;

	[JsonPropertyName("api_itemget_eo_result")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiItemgetEo? ApiItemgetEoResult { get; set; } = default!;

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

	[JsonPropertyName("api_offshore_supply")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiOffshoreSupply? ApiOffshoreSupply { get; set; } = default!;

	[JsonPropertyName("api_production_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiProductionKind { get; set; } = default!;

	[JsonPropertyName("api_rashin_flg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRashinFlg { get; set; } = default!;

	[JsonPropertyName("api_rashin_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRashinId { get; set; } = default!;

	[JsonPropertyName("api_ration_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiRationFlag { get; set; } = default!;

	[JsonPropertyName("api_select_route")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiSelectRoute? ApiSelectRoute { get; set; } = default!;
}
