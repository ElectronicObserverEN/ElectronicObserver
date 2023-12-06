using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class ShipDrop : TsunDbEntity
{
	[JsonProperty("map")]
	public string? Map { get; private set; }

	[JsonProperty("node")]
	public int Node { get; private set; }

	[JsonProperty("rank")]
	public string? Rank { get; private set; }

	[JsonProperty("cleared")]
	public int Cleared { get; private set; }

	[JsonProperty("enemyComp")]
	public EnemyCompShipDrop? EnemyComp { get; private set; }

	[JsonProperty("hqLvl")]
	public int HqLvl { get; private set; }

	[JsonProperty("difficulty")]
	public int Difficulty { get; private set; }

	[JsonProperty("ship")]
	public int Ship { get; private set; }

	[JsonProperty("counts")]
	public Dictionary<int, int> Counts { get; private set; }

	protected override string Url => "drops";

	/// <summary>
	/// Ship drop data
	/// </summary>
	/// <param name="apidata"></param>
	public ShipDrop(dynamic apidata)
	{
		this.Counts = new Dictionary<int, int>();

		PrepareShipCountDictionary();
		PrepareDropData(apidata);
	}

	/// <summary>
	/// Prepare the drop data
	/// </summary>
	private void PrepareDropData(dynamic apidata)
	{
		KCDatabase db = KCDatabase.Instance;

		this.Map = string.Format("{0}-{1}", db.Battle.Compass.MapAreaID, db.Battle.Compass.MapInfoID);
		this.Node = db.Battle.Compass.Destination;
		this.Rank = apidata.api_win_rank;

		MapInfoData mapInfoData = db.MapInfo[db.Battle.Compass.MapAreaID * 10 + db.Battle.Compass.MapInfoID];

		if (mapInfoData == null)
		{
			throw new Exception("Drop data submission : map not found");
		}

		this.Cleared = mapInfoData.IsCleared ? 1 : 0;

		this.HqLvl = db.Admiral.Level;
		this.Difficulty = mapInfoData.EventDifficulty > 0 ? mapInfoData.EventDifficulty : 0;
		this.Ship = (int)(db.Battle.Result.DroppedShipID > 0 ? db.Battle.Result.DroppedShipID : -1);

		PrepareEnemyCompData(apidata);
	}

	/// <summary>
	/// Prepare the data about enemy comp
	/// </summary>
	private void PrepareEnemyCompData(dynamic apidata)
	{
		EnemyComp = new EnemyCompShipDrop(apidata);
		EnemyComp.PrepareEnemyCompFromCurrentState();
	}

	/// <summary>
	/// Fill the Counts dictionnary with the count of every ships with their master id as key
	/// </summary>
	private void PrepareShipCountDictionary()
	{
		foreach (ShipData ship in KCDatabase.Instance.Ships.Values)
		{
			int baseId = ship.MasterShip.BaseShip().ShipID;

			if (this.Counts.ContainsKey(baseId))
				this.Counts[baseId] += 1;
			else
				this.Counts.Add(baseId, 1);
		}
	}
}

public class EnemyCompShipDrop : EnemyComp
{
	[JsonProperty("mapName")]
	public string mapName;

	[JsonProperty("compName")]
	public string compName;

	[JsonProperty("baseExp")]
	public int baseExp;

	public EnemyCompShipDrop(dynamic apidata)
	{
		this.mapName = apidata.api_quest_name;
		this.compName = apidata.api_enemy_info.api_deck_name;
		this.baseExp = (int)apidata.api_get_base_exp;
	}
}
