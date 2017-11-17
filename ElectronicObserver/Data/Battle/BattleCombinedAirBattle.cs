﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 連合艦隊航空戦
	/// </summary>
	public class BattleCombinedAirBattle : BattleDay
	{

		public PhaseAirBattle AirBattle2 { get; protected set; }

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "噴式基地航空隊攻撃");
			JetAirBattle = new PhaseJetAirBattle(this, "噴式航空戦");
			BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空隊攻撃");
			AirBattle = new PhaseAirBattle(this, "第一次航空戦");
			Support = new PhaseSupport(this, "支援攻撃");
			AirBattle2 = new PhaseAirBattle(this, "第二次航空戦", "2");

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);

		}


		public override string APIName => "api_req_combined_battle/airbattle";

		public override string BattleName => ConstantsRes.Title_CombinedFleetAirBattle;

		public override BattleData.BattleTypeFlag BattleType => BattleTypeFlag.Day | BattleTypeFlag.Combined;


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return JetBaseAirAttack;
			yield return JetAirBattle;
			yield return BaseAirAttack;
			yield return AirBattle;
			yield return Support;
			yield return AirBattle2;
		}

	}

}
