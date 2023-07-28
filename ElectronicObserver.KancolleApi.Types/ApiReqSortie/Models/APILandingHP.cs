namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiLandingHp
{
	[JsonPropertyName("api_max_hp")]
	public string ApiMaxHp { get; set; } = "";

	[JsonPropertyName("api_now_hp")]
	public string ApiNowHp { get; set; } = "";

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_sub_value")]
	public object ApiSubValue { get; set; } = "";
}
