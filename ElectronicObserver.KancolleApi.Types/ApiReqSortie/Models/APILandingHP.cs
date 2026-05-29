namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiLandingHp
{
	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_max_hp")]
	public object ApiMaxHp { get; set; } = "";

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_now_hp")]
	public object ApiNowHp { get; set; } = "";

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_sub_value")]
	public object ApiSubValue { get; set; } = "";
}
