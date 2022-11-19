namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiRaigekiClass
{
	[JsonPropertyName("api_ecl")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEcl { get; set; } = new();

	[JsonPropertyName("api_edam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<double> ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiErai { get; set; } = new();

	[JsonPropertyName("api_eydam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEydam { get; set; } = new();

	[JsonPropertyName("api_fcl")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFcl { get; set; } = new();

	[JsonPropertyName("api_fdam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<double> ApiFdam { get; set; } = new();

	[JsonPropertyName("api_frai")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFrai { get; set; } = new();

	[JsonPropertyName("api_fydam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFydam { get; set; } = new();
}
