using System;
using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class ShipDropLoc : TsunDbEntity
{
	[JsonPropertyName("map")]
	public string? Map { get; private set; }

	[JsonPropertyName("node")]
	public int Node { get; private set; }

	[JsonPropertyName("rank")]
	public string? Rank { get; private set; }

	[JsonPropertyName("difficulty")]
	public int Difficulty { get; private set; }

	[JsonPropertyName("ship")]
	public int Ship { get; private set; }

	protected override string Url => "droplocs";

	/// <summary>
	/// Ship drop data
	/// </summary>
	/// <param name="apidata"></param>
	public ShipDropLoc(dynamic apidata)
	{
		PrepareDropData(apidata);
	}

	/// <summary>
	/// Prepare the drop data
	/// </summary>
	private void PrepareDropData(dynamic apidata)
	{
		KCDatabase db = KCDatabase.Instance;

		Map = $"{db.Battle.Compass.MapAreaID}-{db.Battle.Compass.MapInfoID}";
		Node = db.Battle.Compass.CellId;
		Rank = apidata.api_win_rank;

		IMapInfoData mapInfoData = db.MapInfo[db.Battle.Compass.MapAreaID * 10 + db.Battle.Compass.MapInfoID];

		if (mapInfoData is null)
		{
			throw new Exception("Drop data submission : map not found");
		}

		Difficulty = mapInfoData.EventDifficulty > 0 ? mapInfoData.EventDifficulty : 0;
		Ship = db.Battle.Result.DroppedShipID > 0 ? db.Battle.Result.DroppedShipID : -1;
	}
}
