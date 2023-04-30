﻿using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class BattleDay : BattleData
{
	protected PhaseJetBaseAirAttack JetBaseAirAttack { get; }
	protected PhaseJetAirBattle JetAirBattle { get; }
	protected PhaseBaseAirAttack? BaseAirAttack { get; }
	protected PhaseFriendlyAirBattle FriendlyAirBattle { get; }
	protected PhaseAirBattle AirBattle { get; }
	protected PhaseOpeningAsw? OpeningAsw { get; }
	protected PhaseTorpedo OpeningTorpedo { get; }
	protected PhaseShelling Shelling1 { get; }
	protected PhaseShelling Shelling2 { get; }
	protected PhaseShelling Shelling3 { get; }
	protected PhaseTorpedo Torpedo { get; }

	protected BattleDay(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqSortieBattleResponse battle)
		: base(kcDatabase, fleets, battle)
	{
		JetBaseAirAttack = new();
		JetAirBattle = new();
		BaseAirAttack = GetBaseAirAttackPhase(battle.ApiAirBaseAttack);
		FriendlyAirBattle = new();
		AirBattle = new(battle.ApiKouku);
		OpeningAsw = GetOpeningAswPhase(battle.ApiOpeningTaisen);
		OpeningTorpedo = new(battle.ApiOpeningAtack, TorpedoPhase.Opening);
		Shelling1 = new(battle.ApiHougeki1, DayShellingPhase.First);
		Shelling2 = new(battle.ApiHougeki2, DayShellingPhase.Second);
		Shelling3 = new(battle.ApiHougeki3, DayShellingPhase.Third);
		Torpedo = new(battle.ApiRaigeki, TorpedoPhase.Closing);
	}

	private PhaseBaseAirAttack? GetBaseAirAttackPhase(List<ApiAirBaseAttack>? a) => a switch
	{
		null => null,
		_ => new(a),
	};

	private PhaseOpeningAsw? GetOpeningAswPhase(ApiHougeki1? a) => a switch
	{
		null => null,
		_ => new(a),
	};
}
