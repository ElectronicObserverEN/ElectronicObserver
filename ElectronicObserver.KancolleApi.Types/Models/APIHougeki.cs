using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.KancolleApi.Types.Models;

/// <summary>
/// All values will be null when there's a no attack night battle.
/// e.g. sub vs sub
/// </summary>
public class ApiHougeki
{
	[JsonPropertyName("api_at_eflag")]
	public List<FleetFlag>? ApiAtEflag { get; set; }

	[JsonPropertyName("api_at_list")]
	public List<int>? ApiAtList { get; set; }

	[JsonPropertyName("api_cl_list")]
	public List<List<HitType>>? ApiClList { get; set; }

	[JsonPropertyName("api_damage")]
	public List<List<double>>? ApiDamage { get; set; }

	[JsonPropertyName("api_df_list")]
	public List<List<int>>? ApiDfList { get; set; }

	[JsonPropertyName("api_n_mother_list")]
	public List<int>? ApiNMotherList { get; set; }

	/// <summary>
	/// Equipments that get displayed on the screen when the attack happens.
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_si_list")]
	public List<List<object>>? ApiSiList { get; set; }

	[JsonPropertyName("api_sp_list")]
	public List<NightAttackKind>? ApiSpList { get; set; }
}
