namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiRaigekiClass
{
	[JsonPropertyName("api_ecl")]
	[Required]
	public List<int> ApiEcl { get; set; } = new();

	[JsonPropertyName("api_edam")]
	[Required]
	public List<double> ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai")]
	[Required]
	public List<int> ApiErai { get; set; } = new();

	[JsonPropertyName("api_eydam")]
	[Required]
	public List<int> ApiEydam { get; set; } = new();

	[JsonPropertyName("api_fcl")]
	[Required]
	public List<int> ApiFcl { get; set; } = new();

	[JsonPropertyName("api_fdam")]
	[Required]
	public List<double> ApiFdam { get; set; } = new();

	[JsonPropertyName("api_frai")]
	[Required]
	public List<int> ApiFrai { get; set; } = new();

	[JsonPropertyName("api_fydam")]
	[Required]
	public List<int> ApiFydam { get; set; } = new();
}
