﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{
	/// <summary>
	/// 通常/連合艦隊 vs 連合艦隊　夜昼戦
	/// </summary>
	public class BattleEnemyCombinedDayFromNight : BattleDay
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "噴式基地航空隊攻撃");
			JetAirBattle = new PhaseJetAirBattle(this, "噴式航空戦");
			BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空隊攻撃");
			Support = new PhaseSupport(this, "支援攻撃");
			AirBattle = new PhaseAirBattle(this, "航空戦");
			OpeningASW = new PhaseOpeningASW(this, "先制対潜", false);
			OpeningTorpedo = new PhaseTorpedo(this, "先制雷撃", 0);
			Shelling1 = new PhaseShelling(this, "砲撃戦", 1, "1", false);
			Torpedo = new PhaseTorpedo(this, "雷撃戦", 2);

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}

		public override string APIName => "api_req_combined_battle/ec_night_to_day";

		public override string BattleName => "対連合艦隊　夜昼戦";


		public override BattleTypeFlag BattleType => BattleTypeFlag.Day | BattleTypeFlag.EnemyCombined;


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return JetBaseAirAttack;
			yield return JetAirBattle;
			yield return BaseAirAttack;
			yield return AirBattle;
			yield return Support;
			yield return OpeningASW;
			yield return OpeningTorpedo;
			yield return Shelling1;
			yield return Torpedo;
		}
	}
}
