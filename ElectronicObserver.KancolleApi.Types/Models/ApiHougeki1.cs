namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiHougeki1
{
	[JsonPropertyName("api_at_eflag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiAtEflag { get; set; } = new();

	[JsonPropertyName("api_at_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiAtList { get; set; } = new();

	[JsonPropertyName("api_at_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiAtType { get; set; } = new();

	[JsonPropertyName("api_cl_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiClList { get; set; } = new();

	[JsonPropertyName("api_damage")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<double>> ApiDamage { get; set; } = new();

	[JsonPropertyName("api_df_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiDfList { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_si_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<object>> ApiSiList { get; set; } = new();
}
