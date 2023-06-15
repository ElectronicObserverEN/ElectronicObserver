namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiRaigekiClass
{
	[JsonPropertyName("api_ecl")]
	public List<int> ApiEcl { get; set; } = new();

	[JsonPropertyName("api_edam")]
	public List<double> ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai")]
	public List<int> ApiErai { get; set; } = new();

	[JsonPropertyName("api_eydam")]
	public List<int> ApiEydam { get; set; } = new();

	[JsonPropertyName("api_fcl")]
	public List<int> ApiFcl { get; set; } = new();

	[JsonPropertyName("api_fdam")]
	public List<double> ApiFdam { get; set; } = new();

	[JsonPropertyName("api_frai")]
	public List<int> ApiFrai { get; set; } = new();

	[JsonPropertyName("api_fydam")]
	public List<int> ApiFydam { get; set; } = new();
}
