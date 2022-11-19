namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseAttackApiStage3
{
	[JsonPropertyName("api_ebak_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEbakFlag { get; set; } = new();

	[JsonPropertyName("api_ecl_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEclFlag { get; set; } = new();

	[JsonPropertyName("api_edam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<double> ApiEdam { get; set; } = new();

	[JsonPropertyName("api_erai_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEraiFlag { get; set; } = new();
}
