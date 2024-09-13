namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMission
{
	[JsonPropertyName("api_damage_type")]
	public int? ApiDamageType { get; set; }

	[JsonPropertyName("api_deck_num")]
	public int ApiDeckNum { get; set; }

	[JsonPropertyName("api_details")]
	public string ApiDetails { get; set; } = "";

	[JsonPropertyName("api_difficulty")]
	public int ApiDifficulty { get; set; }

	[JsonPropertyName("api_disp_no")]
	public string ApiDispNo { get; set; } = "";

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_maparea_id")]
	public int ApiMapareaId { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_reset_type")]
	public int? ApiResetType { get; set; }

	[JsonPropertyName("api_return_flag")]
	public int ApiReturnFlag { get; set; }

	[JsonPropertyName("api_sample_fleet")]
	public List<int>? ApiSampleFleet { get; set; }

	[JsonPropertyName("api_time")]
	public int ApiTime { get; set; }

	[JsonPropertyName("api_use_bull")]
	public double ApiUseBull { get; set; }

	[JsonPropertyName("api_use_fuel")]
	public double ApiUseFuel { get; set; }

	[JsonPropertyName("api_win_item1")]
	public List<int> ApiWinItem1 { get; set; } = new();

	[JsonPropertyName("api_win_item2")]
	public List<int> ApiWinItem2 { get; set; } = new();

	[JsonPropertyName("api_win_mat_level")]
	public List<int>? ApiWinMatLevel { get; set; }
}
