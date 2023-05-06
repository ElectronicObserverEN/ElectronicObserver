using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

/// <summary>
/// <see cref="ApiFdam"/>, <see cref="ApiFclFlag"/>, <see cref="ApiFbakFlag"/>, <see cref="ApiFraiFlag"/> are null when only enemy fleet is combined <br />
/// <see cref="ApiEdam"/>, <see cref="ApiEclFlag"/>, <see cref="ApiEbakFlag"/>, <see cref="ApiEraiFlag"/> are null when only player fleet is combined
/// </summary>
public class ApiStage3Combined
{
	[JsonPropertyName("api_ebak_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int>? ApiEbakFlag { get; set; }

	[JsonPropertyName("api_ecl_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<AirHitType>? ApiEclFlag { get; set; }

	[JsonPropertyName("api_edam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<double>? ApiEdam { get; set; }

	[JsonPropertyName("api_erai_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int>? ApiEraiFlag { get; set; }

	[JsonPropertyName("api_fbak_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int?>? ApiFbakFlag { get; set; }

	[JsonPropertyName("api_fcl_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<AirHitType>? ApiFclFlag { get; set; }

	[JsonPropertyName("api_fdam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<double>? ApiFdam { get; set; }

	[JsonPropertyName("api_frai_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int?>? ApiFraiFlag { get; set; }
}
