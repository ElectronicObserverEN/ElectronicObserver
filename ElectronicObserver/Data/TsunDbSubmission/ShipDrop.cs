using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class ShipDrop : TsunDbEntity
{
	[JsonPropertyName("map")]
	public string? Map { get; private set; }

	[JsonPropertyName("node")]
	public int Node { get; private set; }

	[JsonPropertyName("rank")]
	public string? Rank { get; private set; }

	[JsonPropertyName("cleared")]
	public int Cleared { get; private set; }

	[JsonPropertyName("enemyComp")]
	public EnemyCompShipDrop? EnemyComp { get; private set; }

	[JsonPropertyName("hqLvl")]
	public int HqLvl { get; private set; }

	[JsonPropertyName("difficulty")]
	public int Difficulty { get; private set; }

	[JsonPropertyName("ship")]
	public int Ship { get; private set; }

	[JsonPropertyName("counts")]
	public Dictionary<int, int> Counts { get; } = new();

	protected override string Url => "drops";

	/// <summary>
	/// Ship drop data
	/// </summary>
	/// <param name="apidata"></param>
	public ShipDrop(dynamic apidata)
	{
		PrepareShipCountDictionary();
		PrepareDropData(apidata);
	}

	/// <summary>
	/// Prepare the drop data
	/// </summary>
	private void PrepareDropData(dynamic apidata)
	{
		KCDatabase db = KCDatabase.Instance;

		Map = $"{db.Battle.Compass.MapAreaID}-{db.Battle.Compass.MapInfoID}";
		Node = db.Battle.Compass.Destination;
		Rank = apidata.api_win_rank;

		MapInfoData mapInfoData = db.MapInfo[db.Battle.Compass.MapAreaID * 10 + db.Battle.Compass.MapInfoID];

		if (mapInfoData == null)
		{
			throw new Exception("Drop data submission : map not found");
		}

		Cleared = mapInfoData.IsCleared ? 1 : 0;

		HqLvl = db.Admiral.Level;
		Difficulty = mapInfoData.EventDifficulty > 0 ? mapInfoData.EventDifficulty : 0;
		Ship = db.Battle.Result.DroppedShipID > 0 ? db.Battle.Result.DroppedShipID : -1;

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

			if (!Counts.TryAdd(baseId, 1))
			{
				Counts[baseId] += 1;
			}
		}
	}
}
