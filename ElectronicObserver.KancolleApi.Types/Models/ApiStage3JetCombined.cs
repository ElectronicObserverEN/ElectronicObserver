using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

/// <summary>
/// <see cref="ApiEdam"/>, <see cref="ApiEclFlag"/>, <see cref="ApiEbakFlag"/>, <see cref="ApiEraiFlag"/> are null when only player fleet is combined
/// </summary>
public class ApiStage3JetCombined
{
	[JsonPropertyName("api_ebak_flag")]
	public List<int>? ApiEbakFlag { get; set; } = new();

	[JsonPropertyName("api_ecl_flag")]
	public List<AirHitType>? ApiEclFlag { get; set; } = new();

	[JsonPropertyName("api_edam")]
	public List<double>? ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai_flag")]
	public List<int>? ApiEraiFlag { get; set; } = new();
}
