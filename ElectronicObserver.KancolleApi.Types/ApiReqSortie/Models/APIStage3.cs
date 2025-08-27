using ElectronicObserver.Core.Types;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiStage3
{
	[JsonPropertyName("api_ebak_flag")]
	public List<int> ApiEbakFlag { get; set; } = new();

	[JsonPropertyName("api_ecl_flag")]
	public List<AirHitType> ApiEclFlag { get; set; } = new();

	[JsonPropertyName("api_edam")]
	public List<double> ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai_flag")]
	public List<int> ApiEraiFlag { get; set; } = new();

	[JsonPropertyName("api_fbak_flag")]
	public List<int?> ApiFbakFlag { get; set; } = new();

	[JsonPropertyName("api_fcl_flag")]
	public List<AirHitType> ApiFclFlag { get; set; } = new();

	[JsonPropertyName("api_fdam")]
	public List<double> ApiFdam { get; set; } = new();

	[JsonPropertyName("api_frai_flag")]
	public List<int?> ApiFraiFlag { get; set; } = new();
}
