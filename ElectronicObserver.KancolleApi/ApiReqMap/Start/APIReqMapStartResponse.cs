using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

public class ApiReqMapStartResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_airsearch")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiAirsearch ApiAirsearch { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_bosscell_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBosscellNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_bosscomp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBosscomp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_cell_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiCellDatum> ApiCellData { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_cell_flavor")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiCellFlavor? ApiCellFlavor { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_color_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiColorNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_event_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiEventId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_event_kind")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiEventKind { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_eventmap")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiEventmap? ApiEventmap { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_from_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFromNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_happening")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiHappening? ApiHappening { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_maparea_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMapareaId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mapinfo_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMapinfoNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_next")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiNext { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_rashin_flg")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRashinFlg { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_rashin_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRashinId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_select_route")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiSelectRoute? ApiSelectRoute { get; set; } = default!;
}
