// ReSharper disable MemberCanBeMadeStatic.Global
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseFactory
{
	private IKCDatabase KcDatabase { get; }

	public PhaseFactory(IKCDatabase kcDatabase)
	{
		KcDatabase = kcDatabase;
	}

	public PhaseInitial Initial(BattleFleets fleets, IBattleApiResponse battle) => battle switch
	{
		ApiDestructionBattle c => new(KcDatabase, fleets, c),
		ICombinedBattleApiResponse c => new(KcDatabase, fleets, c),
		IPlayerCombinedFleetBattle c => new(KcDatabase, fleets, c),
		ICombinedNightBattleApiResponse c => new(KcDatabase, fleets, c),
		IEnemyCombinedFleetBattle c => new(KcDatabase, fleets, c),
		_ => new(KcDatabase, fleets, battle),
	};

	public PhaseSearching Searching(IFirstBattleApiResponse battle) => battle switch
	{
		IDaySearch d => new(d),
		_ => new(battle),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseJetBaseAirAttack? JetBaseAirAttack(ApiAirBaseInjection? a) => a switch
	{
		null => null,
		_ => new(a),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseJetAirBattle? JetAirBattle(ApiInjectionKouku? a) => a switch
	{
		null => null,
		_ => new(a),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseBaseAirAttack? BaseAirAttack(List<ApiAirBaseAttack>? a) => a switch
	{
		null => null,
		_ => new(KcDatabase, a),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseFriendlySupportInfo? FriendlySupportInfo(ApiFriendlyInfo? a) => a switch
	{
		null => null,
		_ => new(a),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseFriendlyAirBattle? FriendlyAirBattle(ApiKouku? a) => a switch
	{
		null => null,
		_ => new(KcDatabase, a),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseAirBattle? AirBattle(ApiKouku? a, AirPhaseType airPhaseType) => a switch
	{
		null => null,
		_ => new(KcDatabase, a, airPhaseType),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseSupport? Support(SupportType supportType, ApiSupportInfo? a, bool isNightSupport) => a switch
	{
		null => null,
		_ => new(supportType, a, isNightSupport),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseOpeningAsw? OpeningAsw(ApiHougeki1? a) => a switch
	{
		null => null,
		_ => new(KcDatabase, a),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseTorpedo? Torpedo(ApiRaigekiClass? a, TorpedoPhase torpedoPhase) => a switch
	{
		null => null,
		_ => new(a, torpedoPhase),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseShelling? Shelling(ApiHougeki1? a, DayShellingPhase shellingPhase) => a switch
	{
		null => null,
		_ => new(KcDatabase, a, shellingPhase),
	};

	public PhaseNightInitial NightInitial(BattleFleets fleets, INightGearApiResponse battle) => battle switch
	{
		ICombinedNightBattleApiResponse c => new(KcDatabase, fleets, c),
		_ => new(KcDatabase, fleets, battle),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseFriendlyShelling? FriendlyShelling(ApiFriendlyBattle? a) => a switch
	{
		null => null,
		_ => new(KcDatabase, a),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseNightBattle? NightBattle(ApiHougeki? a) => a switch
	{
		{
			ApiAtEflag: { } apiAtEflag,
			ApiAtList: { } apiAtList,
			ApiClList: { } apiClList,
			ApiDamage: { } apiDamage,
			ApiDfList: { } apiDfList,
			ApiNMotherList: { } apiNMotherList,
			ApiSiList: { } apiSiList,
			ApiSpList: { } apiSpList,
		} => new(KcDatabase, apiAtEflag, apiAtList, apiClList, apiDamage, apiDfList, apiNMotherList, apiSiList, apiSpList),

		_ => null,
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseRadar? Radar(ApiHougeki1? a) => a switch
	{
		null => null,
		_ => new(KcDatabase, a),
	};

	[return: NotNullIfNotNull(nameof(a))]
	public PhaseBaseAirRaid? BaseAirRaid(ApiAirBaseRaid? a) => a switch
	{
		null => null,
		_ => new(a),
	};
}
