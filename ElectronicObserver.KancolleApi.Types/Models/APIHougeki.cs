﻿using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiHougeki
{
	[JsonPropertyName("api_at_eflag")]
	public List<FleetFlag> ApiAtEflag { get; set; } = new();

	[JsonPropertyName("api_at_list")]
	public List<int> ApiAtList { get; set; } = new();

	[JsonPropertyName("api_cl_list")]
	public List<List<HitType>> ApiClList { get; set; } = new();

	[JsonPropertyName("api_damage")]
	public List<List<double>> ApiDamage { get; set; } = new();

	[JsonPropertyName("api_df_list")]
	public List<List<int>> ApiDfList { get; set; } = new();

	[JsonPropertyName("api_n_mother_list")]
	public List<int> ApiNMotherList { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_si_list")]
	public List<List<object>> ApiSiList { get; set; } = new();

	[JsonPropertyName("api_sp_list")]
	public List<NightAttackKind> ApiSpList { get; set; } = new();
}
