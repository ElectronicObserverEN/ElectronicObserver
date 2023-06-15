namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMission
{
	[JsonPropertyName("api_damage_type")]
	public int? ApiDamageType { get; set; } = default!;

	[JsonPropertyName("api_deck_num")]
	public int ApiDeckNum { get; set; } = default!;

	[JsonPropertyName("api_details")]
	public string ApiDetails { get; set; } = default!;

	[JsonPropertyName("api_difficulty")]
	public int ApiDifficulty { get; set; } = default!;

	[JsonPropertyName("api_disp_no")]
	public string ApiDispNo { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_maparea_id")]
	public int ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_reset_type")]
	public int? ApiResetType { get; set; } = default!;

	[JsonPropertyName("api_return_flag")]
	public int ApiReturnFlag { get; set; } = default!;

	[JsonPropertyName("api_sample_fleet")]
	public List<int>? ApiSampleFleet { get; set; } = default!;

	[JsonPropertyName("api_time")]
	public int ApiTime { get; set; } = default!;

	[JsonPropertyName("api_use_bull")]
	public int ApiUseBull { get; set; } = default!;

	[JsonPropertyName("api_use_fuel")]
	public int ApiUseFuel { get; set; } = default!;

	[JsonPropertyName("api_win_item1")]
	public List<int> ApiWinItem1 { get; set; } = new();

	[JsonPropertyName("api_win_item2")]
	public List<int> ApiWinItem2 { get; set; } = new();

	[JsonPropertyName("api_win_mat_level")]
	public List<int>? ApiWinMatLevel { get; set; } = default!;
}
