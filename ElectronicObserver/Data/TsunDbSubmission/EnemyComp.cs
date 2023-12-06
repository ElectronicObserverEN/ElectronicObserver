using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data.Battle.Phase;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class EnemyComp : TsunDbEntity
{
	protected override string Url => "";

	[JsonProperty("ship")]
	public List<int> Ship { get; private set; } = new List<int>();

	[JsonProperty("lvl")]
	public List<int> Lvl { get; private set; } = new List<int>();

	[JsonProperty("hp")]
	public List<int> HP { get; private set; } = new List<int>();

	[JsonProperty("stats")]
	public List<int[]> Stats { get; private set; } = new List<int[]>();

	[JsonProperty("equip")]
	public List<int[]> Equips { get; private set; } = new List<int[]>();

	[JsonProperty("formation")]
	public int Formation { get; private set; }

	[JsonProperty("isAirRaid")]
	public bool IsAirRaid { get; private set; }


	[JsonProperty("gaugeNum")]
	public int GaugeNum { get; private set; }

	[JsonProperty("currentHP")]
	public int CurrentHP { get; private set; }

	[JsonProperty("maxHP")]
	public int MaxHP { get; private set; }


	[JsonProperty("shipEscort")]
	public List<int>? ShipEscort { get; private set; }

	[JsonProperty("lvlEscort")]
	public List<int>? LvlEscort { get; private set; }

	[JsonProperty("hpEscort")]
	public List<int>? HPEscort { get; private set; }

	[JsonProperty("statsEscort")]
	public List<int[]>? StatsEscort { get; private set; }

	[JsonProperty("equipEscort")]
	public List<int[]>? EquipsEscort { get; private set; }

	public EnemyComp()
	{

	}

	public void PrepareEnemyCompFromCurrentState()
	{
		KCDatabase db = KCDatabase.Instance;
		PhaseInitial initial = db.Battle.FirstBattle.Initial;

		int shipCount = initial.EnemyMembers.Count((id) => { return id != -1; });

		Ship = initial.EnemyMembers.Take(shipCount).ToList();
		Lvl = initial.EnemyLevels.Take(shipCount).ToList();
		HP = initial.EnemyMaxHPs.Take(shipCount).ToList();
		Stats = initial.EnemyParameters.Take(shipCount).ToList();
		Equips = initial.EnemySlots.Take(shipCount).ToList();
		Formation = db.Battle.FirstBattle.Searching.FormationEnemy;

		// --- If this is an event map
		if (db.Battle.Compass.MapAreaID > 30)
		{
			MapInfoData mapInfoData = db.MapInfo[db.Battle.Compass.MapAreaID * 10 + db.Battle.Compass.MapInfoID];

			GaugeNum = mapInfoData.CurrentGaugeIndex;
			CurrentHP = mapInfoData.MapHPCurrent;
			MaxHP = mapInfoData.MapHPMax;
		}

		// --- If enemy fleet is combined
		if (initial.IsEnemyCombined)
		{
			shipCount = initial.EnemyMembersEscort.Count((id) => { return id != -1; });

			ShipEscort = initial.EnemyMembersEscort.Take(shipCount).ToList();
			LvlEscort = initial.EnemyLevelsEscort.Take(shipCount).ToList();
			HPEscort = initial.EnemyMaxHPsEscort.Take(shipCount).ToList();
			StatsEscort = initial.EnemyParametersEscort.Take(shipCount).ToList();
			EquipsEscort = initial.EnemySlotsEscort.Take(shipCount).ToList();
		}

		IsAirRaid = db.Battle.BattleMode == Data.Battle.BattleManager.BattleModes.AirRaid;
	}
}
