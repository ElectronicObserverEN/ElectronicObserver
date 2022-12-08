namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiStage3
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

	[JsonPropertyName("api_fbak_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int?> ApiFbakFlag { get; set; } = new();

	[JsonPropertyName("api_fcl_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFclFlag { get; set; } = new();

	[JsonPropertyName("api_fdam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<double> ApiFdam { get; set; } = new();

	[JsonPropertyName("api_frai_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int?> ApiFraiFlag { get; set; } = new();
}
