using ElectronicObserver.KancolleApi.Types.ApiReqMission.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;

public class ApiReqMissionResultResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_clear_result")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiClearResult { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_detail")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiDetail { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGetExp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_exp_lvup")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_get_item1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiGetItem? ApiGetItem1 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_item2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiGetItem? ApiGetItem2 { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="List{T}"/> of <see cref="int"/>s.
	/// </summary>
	[System.Text.Json.Serialization.JsonPropertyName("api_get_material")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public object ApiGetMaterial { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_ship_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiGetShipExp { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_maparea_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiMapareaName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberExp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_lv")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberLv { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_quest_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiQuestLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_quest_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiQuestName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipId { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_useitem_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiUseitemFlag { get; set; } = new();
}
