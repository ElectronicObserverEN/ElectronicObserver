using ElectronicObserver.KancolleApi.Types.ApiReqMission.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;

public class ApiReqMissionResultResponse
{
	[JsonPropertyName("api_clear_result")]
	public int ApiClearResult { get; set; }

	[JsonPropertyName("api_detail")]
	public string ApiDetail { get; set; } = "";

	[JsonPropertyName("api_get_exp")]
	public int ApiGetExp { get; set; }

	[JsonPropertyName("api_get_exp_lvup")]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_item1")]
	public ApiMissionGetItem? ApiGetItem1 { get; set; }

	[JsonPropertyName("api_get_item2")]
	public ApiMissionGetItem? ApiGetItem2 { get; set; }

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="List{T}"/> of <see cref="int"/>s.
	/// </summary>
	[JsonPropertyName("api_get_material")]
	public object ApiGetMaterial { get; set; } = -1;

	[JsonPropertyName("api_get_ship_exp")]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_maparea_name")]
	public string ApiMapareaName { get; set; } = "";

	[JsonPropertyName("api_member_exp")]
	public int ApiMemberExp { get; set; }

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_member_lv")]
	public object ApiMemberLv { get; set; } = 0;

	[JsonPropertyName("api_quest_level")]
	public int ApiQuestLevel { get; set; }

	[JsonPropertyName("api_quest_name")]
	public string ApiQuestName { get; set; } = "";

	[JsonPropertyName("api_ship_id")]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_useitem_flag")]
	public List<int> ApiUseitemFlag { get; set; } = new();
}
