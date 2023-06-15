using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

/// <summary>
/// <see cref="ApiEdam"/>, <see cref="ApiEclFlag"/>, <see cref="ApiEbakFlag"/>, <see cref="ApiEraiFlag"/> are null when only player fleet is combined
/// </summary>
public class ApiStage3JetCombined
{
	[JsonPropertyName("api_ebak_flag")]
	[Required]
	public List<int>? ApiEbakFlag { get; set; } = new();

	[JsonPropertyName("api_ecl_flag")]
	[Required]
	public List<AirHitType>? ApiEclFlag { get; set; } = new();

	[JsonPropertyName("api_edam")]
	[Required]
	public List<double>? ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai_flag")]
	[Required]
	public List<int>? ApiEraiFlag { get; set; } = new();
}
