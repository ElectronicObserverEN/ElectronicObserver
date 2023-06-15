using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiHougeki1
{
	[JsonPropertyName("api_at_eflag")]
	[Required]
	public List<FleetFlag> ApiAtEflag { get; set; } = new();

	[JsonPropertyName("api_at_list")]
	[Required]
	public List<int> ApiAtList { get; set; } = new();

	[JsonPropertyName("api_at_type")]
	[Required]
	public List<DayAttackKind> ApiAtType { get; set; } = new();

	[JsonPropertyName("api_cl_list")]
	[Required]
	public List<List<HitType>> ApiClList { get; set; } = new();

	[JsonPropertyName("api_damage")]
	[Required]
	public List<List<double>> ApiDamage { get; set; } = new();

	[JsonPropertyName("api_df_list")]
	[Required]
	public List<List<int>> ApiDfList { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_si_list")]
	[Required]
	public List<List<object>> ApiSiList { get; set; } = new();
}
