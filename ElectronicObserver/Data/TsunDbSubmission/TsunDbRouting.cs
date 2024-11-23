using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbRouting : TsunDbEntity
{
	protected override string Url => "routing";

	#region Json Properties
	[JsonPropertyName("sortiedFleet")]
	public int SortiedFleet { get; private set; }

	[JsonPropertyName("fleetType")]
	public int FleetType { get; private set; }

	[JsonPropertyName("nodeInfo")]
	public TsunDbNodeInfo NodeInfo { get; private set; } = new(0);

	[JsonPropertyName("map")] 
	public string Map { get; private set; } = "";

	[JsonPropertyName("hqLvl")]
	public int HqLvl { get; private set; }

	[JsonPropertyName("cleared")]
	public bool Cleared { get; private set; }

	[JsonPropertyName("edgeID")]
	public List<int> EdgeID { get; } = new();

	[JsonPropertyName("nextRoute")]
	public int NextRoute { get; private set; }

	[JsonPropertyName("fleet1")]
	public List<TsunDbShipData> Fleet1 { get; private set; } = new();

	[JsonPropertyName("fleet2")]
	public List<TsunDbShipData> Fleet2 { get; private set; } = new();

	[JsonPropertyName("fleetSpeed")]
	public int FleetSpeed { get; private set; }

	[JsonPropertyName("los")]
	public double[] LOS { get; private set; } = { 0, 0, 0, 0 };

	[JsonPropertyName("fleetids")]
	public List<int> FleetIds { get; private set; } = new();

	[JsonPropertyName("fleetlevel")]
	public int FleetLevel { get; private set; }

	[JsonPropertyName("fleetoneequips")]
	public List<int> FleetOneEquips { get; private set; } = new();

	[JsonPropertyName("fleetoneexslots")]
	public List<int> FleetOneExSlots { get; private set; } = new();

	[JsonPropertyName("fleetonetypes")]
	public List<int> FleetOneTypes { get; private set; } = new();

	[JsonPropertyName("fleettwoequips")]
	public List<int> FleetTwoEquips { get; private set; } = new();

	[JsonPropertyName("fleettwoexslots")]
	public List<int> FleetTwoExSlots { get; private set; } = new();

	[JsonPropertyName("fleettwotypes")]
	public List<int> FleetTwoTypes { get; private set; } = new();

	#endregion

	public TsunDbRouting()
	{
		// Need start call to initialize
		IsInitialized = false;
	}

	#region public methods
	/// <summary>
	/// Process Start node request
	/// </summary>
	/// <param name="apiData"></param>
	public void ProcessStart(dynamic apiData)
	{
		KCDatabase db = KCDatabase.Instance;

		SortiedFleet = db.Fleet.Fleets
			.Where(fleet => fleet.Value.IsInSortie)
			.Select(fleet => fleet.Value.FleetID)
			.Min();

		// Get the fleet type, if first fleet => flag of the combined fleet, else 0 (single fleet & strike force)
		FleetType = SortiedFleet == 1 ? (int)db.Fleet.CombinedFlag : 0;

		// Sets amount of nodes value in NodeInfo
		object[] cellData = apiData["api_cell_data"];
		NodeInfo = new TsunDbNodeInfo(cellData.Length);

		// Sets the map value
		Map = $"{db.Battle.Compass.MapAreaID}-{db.Battle.Compass.MapInfoID}";

		// LBAS THINGS
		//this.processCellData(http); TODO

		IsInitialized = true;

		ProcessNext(apiData);
	}

	/// <summary>
	/// Process next node request
	/// </summary>
	/// <param name="apiData"></param>
	public void ProcessNext(dynamic apiData)
	{
		// Some data is initiaized by Start api
		// In some case it's missing (Enabling tsundb midsortie)
		if (!IsInitialized) return;

		CleanOnNext();

		KCDatabase db = KCDatabase.Instance;

		// Sets player's HQ level
		HqLvl = db.Admiral.Level;

		// Sets whether the map is cleared or not
		Cleared = db.Battle.Compass.MapInfo.IsCleared;

		// Charts the route array using edge ids as values
		EdgeID.Add((int)apiData.api_no);

		// All values related to node types
		NodeInfo.ProcessNext(apiData);

		// Checks whether the fleet has hit a dead end or not
		NextRoute = (int)apiData.api_next;

		// Fleet 1
		Fleet1 = PrepareFleet(db.Fleet[SortiedFleet]);

		// Fleet 2
		if (FleetType > 0 && SortiedFleet == 1)
		{
			Fleet2 = PrepareFleet(db.Fleet[2]);
		}

		foreach (TsunDbShipData? ship in Fleet1)
		{
			FleetIds.Add(ship.Id);
			FleetLevel += ship.Level;
			FleetOneEquips.AddRange(ship.Equip);
			FleetOneExSlots.Add(ship.Exslot);
			FleetOneTypes.Add(ship.Type);
		}

		if (Fleet2.Count > 0)
		{
			foreach (TsunDbShipData? ship in Fleet2)
			{
				FleetIds.Add(ship.Id);
				FleetLevel += ship.Level;
				FleetTwoEquips.AddRange(ship.Equip);
				FleetTwoExSlots.Add(ship.Exslot);
				FleetTwoTypes.Add(ship.Type);
			}
		}
	}

	#endregion

	#region Private methods
	/// <summary>
	/// Clean all data before loading a node data
	/// </summary>
	private void CleanOnNext()
	{
		LOS = new double[] { 0, 0, 0, 0 };
		Fleet1 = new();
		Fleet2 = new();
		FleetSpeed = 20;
		FleetIds = new();
		FleetLevel = 0;
		FleetOneEquips = new();
		FleetOneExSlots = new();
		FleetOneTypes = new();
		FleetTwoEquips = new();
		FleetTwoExSlots = new();
		FleetTwoTypes = new();
	}

	/// <summary>
	/// Prepare fleet data
	/// </summary>
	/// <param name="fleetData"></param>
	/// <returns></returns>
	private List<TsunDbShipData> PrepareFleet(IFleetData fleetData)
	{
		IEnumerable<IShipData> members = fleetData.MembersWithoutEscaped?.Where(s => s != null).Cast<IShipData>().ToList() ?? new();

		FleetSpeed = Math.Min(FleetSpeed, members.Select(s => s.Speed).Min());

		foreach (int weight in new[] { 1, 2, 3, 4 })
		{
			LOS[weight - 1] += Calculator.GetSearchingAbility_New33(fleetData, weight, HqLvl);
		}

		return fleetData.MembersInstance
			.Where(ship => ship is not null)
			.Cast<IShipData>()
			.Select(ship => new TsunDbShipData(ship) { Flee = fleetData.EscapedShipList.Contains(ship.MasterID) })
			.ToList();
	}
	#endregion
}
