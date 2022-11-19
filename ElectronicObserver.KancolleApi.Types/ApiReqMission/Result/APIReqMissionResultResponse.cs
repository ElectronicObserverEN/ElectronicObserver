using ElectronicObserver.KancolleApi.Types.ApiReqMission.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;

public class ApiReqMissionResultResponse
{
	[JsonPropertyName("api_clear_result")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiClearResult { get; set; } = default!;

	[JsonPropertyName("api_detail")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDetail { get; set; } = default!;

	[JsonPropertyName("api_get_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGetExp { get; set; } = default!;

	[JsonPropertyName("api_get_exp_lvup")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_item1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiGetItem? ApiGetItem1 { get; set; } = default!;

	[JsonPropertyName("api_get_item2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiGetItem? ApiGetItem2 { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="List{T}"/> of <see cref="int"/>s.
	/// </summary>
	[JsonPropertyName("api_get_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object ApiGetMaterial { get; set; } = default!;

	[JsonPropertyName("api_get_ship_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_maparea_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiMapareaName { get; set; } = default!;

	[JsonPropertyName("api_member_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberExp { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_member_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object ApiMemberLv { get; set; } = default!;

	[JsonPropertyName("api_quest_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiQuestLevel { get; set; } = default!;

	[JsonPropertyName("api_quest_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiQuestName { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_useitem_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiUseitemFlag { get; set; } = new();
}
