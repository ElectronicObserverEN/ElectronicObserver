using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage3Jet
{
	[JsonPropertyName("api_ebak_flag")]
	[Required]
	public List<int> ApiEbakFlag { get; set; } = new();

	[JsonPropertyName("api_ecl_flag")]
	[Required]
	public List<AirHitType> ApiEclFlag { get; set; } = new();

	[JsonPropertyName("api_edam")]
	[Required]
	public List<double> ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai_flag")]
	[Required]
	public List<int> ApiEraiFlag { get; set; } = new();
}
