using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

/// <summary>
/// <see cref="ApiFdam"/>, <see cref="ApiFclFlag"/>, <see cref="ApiFbakFlag"/>, <see cref="ApiFraiFlag"/> are null when only enemy fleet is combined <br />
/// <see cref="ApiEdam"/>, <see cref="ApiEclFlag"/>, <see cref="ApiEbakFlag"/>, <see cref="ApiEraiFlag"/> are null when only player fleet is combined
/// </summary>
public class ApiStage3Combined
{
	[JsonPropertyName("api_ebak_flag")]
	[Required]
	public List<int>? ApiEbakFlag { get; set; }

	[JsonPropertyName("api_ecl_flag")]
	[Required]
	public List<AirHitType>? ApiEclFlag { get; set; }

	[JsonPropertyName("api_edam")]
	[Required]
	public List<double>? ApiEdam { get; set; }

	[JsonPropertyName("api_erai_flag")]
	[Required]
	public List<int>? ApiEraiFlag { get; set; }

	[JsonPropertyName("api_fbak_flag")]
	[Required]
	public List<int?>? ApiFbakFlag { get; set; }

	[JsonPropertyName("api_fcl_flag")]
	[Required]
	public List<AirHitType>? ApiFclFlag { get; set; }

	[JsonPropertyName("api_fdam")]
	[Required]
	public List<double>? ApiFdam { get; set; }

	[JsonPropertyName("api_frai_flag")]
	[Required]
	public List<int?>? ApiFraiFlag { get; set; }
}
