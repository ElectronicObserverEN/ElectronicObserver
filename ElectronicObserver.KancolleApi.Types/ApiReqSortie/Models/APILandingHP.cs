namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiLandingHp
{
	[JsonPropertyName("api_max_hp")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMaxHp { get; set; } = default!;

	[JsonPropertyName("api_now_hp")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNowHp { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_sub_value")]
	public object ApiSubValue { get; set; } = default!;
}
