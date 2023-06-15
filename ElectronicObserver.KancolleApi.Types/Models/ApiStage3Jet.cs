using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage3Jet
{
	[JsonPropertyName("api_ebak_flag")]
	public List<int> ApiEbakFlag { get; set; } = new();

	[JsonPropertyName("api_ecl_flag")]
	public List<AirHitType> ApiEclFlag { get; set; } = new();

	[JsonPropertyName("api_edam")]
	public List<double> ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai_flag")]
	public List<int> ApiEraiFlag { get; set; } = new();
}
