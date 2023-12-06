using System;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class ShipDropLoc : TsunDbEntity
{
	[JsonProperty("map")]
	public string? Map { get; private set; }

	[JsonProperty("node")]
	public int Node { get; private set; }

	[JsonProperty("rank")]
	public string? Rank { get; private set; }

	[JsonProperty("difficulty")]
	public int Difficulty { get; private set; }

	[JsonProperty("ship")]
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

		this.Map = string.Format("{0}-{1}", db.Battle.Compass.MapAreaID, db.Battle.Compass.MapInfoID);
		this.Node = db.Battle.Compass.Destination;
		this.Rank = apidata.api_win_rank;

		MapInfoData mapInfoData = db.MapInfo[db.Battle.Compass.MapAreaID * 10 + db.Battle.Compass.MapInfoID];

		if (mapInfoData == null)
		{
			throw new Exception("Drop data submission : map not found");
		}

		this.Difficulty = mapInfoData.EventDifficulty > 0 ? mapInfoData.EventDifficulty : 0;
		this.Ship = (int)(db.Battle.Result.DroppedShipID > 0 ? db.Battle.Result.DroppedShipID : -1);
	}
}
