namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMission
{
	[JsonPropertyName("api_damage_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiDamageType { get; set; } = default!;

	[JsonPropertyName("api_deck_num")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDeckNum { get; set; } = default!;

	[JsonPropertyName("api_details")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDetails { get; set; } = default!;

	[JsonPropertyName("api_difficulty")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDifficulty { get; set; } = default!;

	[JsonPropertyName("api_disp_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDispNo { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_maparea_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_reset_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiResetType { get; set; } = default!;

	[JsonPropertyName("api_return_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiReturnFlag { get; set; } = default!;

	[JsonPropertyName("api_sample_fleet")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiSampleFleet { get; set; } = default!;

	[JsonPropertyName("api_time")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiTime { get; set; } = default!;

	[JsonPropertyName("api_use_bull")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUseBull { get; set; } = default!;

	[JsonPropertyName("api_use_fuel")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUseFuel { get; set; } = default!;

	[JsonPropertyName("api_win_item1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiWinItem1 { get; set; } = new();

	[JsonPropertyName("api_win_item2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiWinItem2 { get; set; } = new();

	[JsonPropertyName("api_win_mat_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiWinMatLevel { get; set; } = default!;
}
