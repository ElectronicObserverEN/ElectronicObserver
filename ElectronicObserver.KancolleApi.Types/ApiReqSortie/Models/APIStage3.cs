using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiStage3
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

	[JsonPropertyName("api_fbak_flag")]
	[Required]
	public List<int?> ApiFbakFlag { get; set; } = new();

	[JsonPropertyName("api_fcl_flag")]
	[Required]
	public List<AirHitType> ApiFclFlag { get; set; } = new();

	[JsonPropertyName("api_fdam")]
	[Required]
	public List<double> ApiFdam { get; set; } = new();

	[JsonPropertyName("api_frai_flag")]
	[Required]
	public List<int?> ApiFraiFlag { get; set; } = new();
}
