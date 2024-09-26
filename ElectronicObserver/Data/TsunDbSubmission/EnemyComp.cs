using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class EnemyComp : TsunDbEntity
{
	protected override string Url => throw new NotImplementedException();

	[JsonPropertyName("ship")]
	public List<int> Ship { get; private set; } = new();

	[JsonPropertyName("lvl")]
	public List<int> Lvl { get; private set; } = new();

	[JsonPropertyName("hp")]
	public List<int> HP { get; private set; } = new();

	[JsonPropertyName("stats")]
	public List<List<int>> Stats { get; private set; } = new();

	[JsonPropertyName("equip")]
	public List<int[]> Equips { get; private set; } = new();

	[JsonPropertyName("formation")]
	public int Formation { get; private set; }

	[JsonPropertyName("isAirRaid")]
	public bool IsAirRaid { get; private set; }


	[JsonPropertyName("gaugeNum")]
	public int GaugeNum { get; private set; }

	[JsonPropertyName("currentHP")]
	public int CurrentHP { get; private set; }

	[JsonPropertyName("maxHP")]
	public int MaxHP { get; private set; }


	[JsonPropertyName("shipEscort")]
	public List<int>? ShipEscort { get; private set; }

	[JsonPropertyName("lvlEscort")]
	public List<int>? LvlEscort { get; private set; }

	[JsonPropertyName("hpEscort")]
	public List<int>? HPEscort { get; private set; }

	[JsonPropertyName("statsEscort")]
	public List<List<int>>? StatsEscort { get; private set; }

	[JsonPropertyName("equipEscort")]
	public List<int[]>? EquipsEscort { get; private set; }
	
	public void PrepareEnemyCompFromCurrentState()
	{
		KCDatabase db = KCDatabase.Instance;
		PhaseInitial initial = db.Battle.FirstBattle.Initial;

		int shipCount = initial.EnemyMembers.Count((id) => id != -1);

		Ship = initial.EnemyMembers.Take(shipCount).ToList();
		Lvl = initial.EnemyLevels.Take(shipCount).ToList();
		HP = initial.EnemyMaxHPs.Take(shipCount).ToList();
		Stats = initial.EnemyParameters.Take(shipCount).ToList();
		Equips = initial.EnemySlots.Take(shipCount).ToList();
		Formation = (int)db.Battle.FirstBattle.Searching.EnemyFormationType;

		// If this is an event map
		if (db.Battle.Compass.MapAreaID > 30)
		{
			MapInfoData mapInfoData = db.MapInfo[db.Battle.Compass.MapAreaID * 10 + db.Battle.Compass.MapInfoID];

			GaugeNum = mapInfoData.CurrentGaugeIndex;
			CurrentHP = mapInfoData.MapHPCurrent;
			MaxHP = mapInfoData.MapHPMax;
		}

		// If enemy fleet is combined
		if (initial.IsEnemyCombined)
		{
			shipCount = initial.EnemyMembersEscort?.Count((id) => id != -1) ?? 0;

			ShipEscort = initial.EnemyMembersEscort?.Take(shipCount).ToList();
			LvlEscort = initial.EnemyLevelsEscort?.Take(shipCount).ToList();
			HPEscort = initial.EnemyMaxHPsEscort?.Take(shipCount).ToList();
			StatsEscort = initial.EnemyParametersEscort.Take(shipCount).ToList();
			EquipsEscort = initial.EnemySlotsEscort.Take(shipCount).ToList();
		}

		IsAirRaid = db.Battle.IsAirRaid;
	}
}
