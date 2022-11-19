namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage3Combined
{
	[JsonPropertyName("api_fbak_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFbakFlag { get; set; } = new();

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
	public List<int> ApiFraiFlag { get; set; } = new();
}
