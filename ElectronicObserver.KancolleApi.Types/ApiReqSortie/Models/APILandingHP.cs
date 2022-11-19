namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiLandingHp
{
	[JsonPropertyName("api_max_hp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiMaxHp { get; set; } = default!;

	[JsonPropertyName("api_now_hp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNowHp { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_sub_value")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object ApiSubValue { get; set; } = default!;
}
