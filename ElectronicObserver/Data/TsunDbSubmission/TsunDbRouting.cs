using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbRouting : TsunDbEntity
{
	protected override string Url => "routing";

	#region Json Properties
	[JsonProperty("sortiedFleet")]
	public int SortiedFleet { get; private set; }

	[JsonProperty("fleetType")]
	public int FleetType { get; private set; }

	[JsonProperty("nodeInfo")]
	public TsunDbNodeInfo NodeInfo { get; private set; }

	[JsonProperty("map")]
	public string Map { get; private set; }

	[JsonProperty("hqLvl")]
	public int HqLvl { get; private set; }

	[JsonProperty("cleared")]
	public bool Cleared { get; private set; }

	[JsonProperty("edgeID")]
	public List<int> EdgeID { get; private set; }

	[JsonProperty("nextRoute")]
	public int NextRoute { get; private set; }

	[JsonProperty("fleet1")]
	public List<TsunDbShipData> Fleet1 { get; private set; }

	[JsonProperty("fleet2")]
	public List<TsunDbShipData> Fleet2 { get; private set; }

	[JsonProperty("fleetSpeed")]
	public int FleetSpeed { get; private set; }

	[JsonProperty("los")]
	public double[] LOS { get; private set; }

	[JsonProperty("fleetids")]
	public List<int> FleetIds { get; private set; }

	[JsonProperty("fleetlevel")]
	public int FleetLevel { get; private set; }

	[JsonProperty("fleetoneequips")]
	public List<int> FleetOneEquips { get; private set; }

	[JsonProperty("fleetoneexslots")]
	public List<int> FleetOneExSlots { get; private set; }

	[JsonProperty("fleetonetypes")]
	public List<int> FleetOneTypes { get; private set; }

	[JsonProperty("fleettwoequips")]
	public List<int> FleetTwoEquips { get; private set; }

	[JsonProperty("fleettwoexslots")]
	public List<int> FleetTwoExSlots { get; private set; }

	[JsonProperty("fleettwotypes")]
	public List<int> FleetTwoTypes { get; private set; }
	#endregion

	public TsunDbRouting() : base()
	{
		FleetTwoTypes = new List<int>();
		FleetTwoExSlots = new List<int>();
		FleetTwoEquips = new List<int>();
		FleetOneTypes = new List<int>();
		FleetOneExSlots = new List<int>();
		FleetOneEquips = new List<int>();
		EdgeID = new List<int>();
		FleetIds = new List<int>();
		LOS = new double[] { 0, 0, 0, 0 };
		Fleet1 = new List<TsunDbShipData>();
		Fleet2 = new List<TsunDbShipData>();
		NodeInfo = new TsunDbNodeInfo(0);
		Map = string.Empty;

		// Need start call to initialize
		IsInitialized = false;
	}

	#region public methods
	/// <summary>
	/// Process Start node request
	/// </summary>
	/// <param name="api_data"></param>
	public void ProcessStart(dynamic api_data)
	{
		KCDatabase db = KCDatabase.Instance;

		this.SortiedFleet = db.Fleet.Fleets
			.Where(fleet => fleet.Value.IsInSortie)
			.Select(fleet => fleet.Value.FleetID)
			.Min();

		// --- Get the fleet type, if first fleet => flag of the combined fleet, else 0 (single fleet & strike force)
		FleetType = SortiedFleet == 1 ? db.Fleet.CombinedFlag : 0;

		// --- Sets amount of nodes value in NodeInfo
		object[] cell_data = api_data["api_cell_data"];
		this.NodeInfo = new TsunDbNodeInfo(cell_data.Length);

		// --- Sets the map value
		this.Map = string.Format("{0}-{1}", db.Battle.Compass.MapAreaID, db.Battle.Compass.MapInfoID);

		// LBAS THINGS
		//this.processCellData(http); TODO

		IsInitialized = true;

		this.ProcessNext(api_data);
	}

	/// <summary>
	/// Process next node request
	/// </summary>
	/// <param name="api_data"></param>
	public void ProcessNext(dynamic api_data)
	{
		// Some data is initiaized by Start api
		// In some case it's missing (Enabling tsundb midsortie)
		if (!IsInitialized) return;

		CleanOnNext();

		KCDatabase db = KCDatabase.Instance;

		// --- Sets player's HQ level
		this.HqLvl = db.Admiral.Level;

		// --- Sets whether the map is cleared or not
		this.Cleared = db.Battle.Compass.MapInfo.IsCleared;

		// --- Charts the route array using edge ids as values
		this.EdgeID.Add((int)api_data.api_no);

		// --- All values related to node types
		this.NodeInfo.ProcessNext(api_data);

		// --- Checks whether the fleet has hit a dead end or not
		this.NextRoute = (int)api_data.api_next;

		// --- Fleet 1
		this.Fleet1 = this.PrepareFleet(db.Fleet[this.SortiedFleet]);

		// --- Fleet 2
		if (this.FleetType > 0 && this.SortiedFleet == 1)
		{
			this.Fleet2 = this.PrepareFleet(db.Fleet[2]);
		}

		foreach (var ship in this.Fleet1)
		{
			this.FleetIds.Add(ship.Id);
			this.FleetLevel += ship.Level;
			this.FleetOneEquips.AddRange(ship.Equip);
			this.FleetOneExSlots.Add(ship.Exslot);
			this.FleetOneTypes.Add(ship.Type);
		}


		if (this.Fleet2.Count > 0)
		{
			foreach (var ship in this.Fleet2)
			{
				this.FleetIds.Add(ship.Id);
				this.FleetLevel += ship.Level;
				this.FleetTwoEquips.AddRange(ship.Equip);
				this.FleetTwoExSlots.Add(ship.Exslot);
				this.FleetTwoTypes.Add(ship.Type);
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
		this.LOS = new double[] { 0, 0, 0, 0 };
		this.Fleet1 = new List<TsunDbShipData>();
		this.Fleet2 = new List<TsunDbShipData>();
		this.FleetSpeed = 20;
		this.FleetIds = new List<int>();
		this.FleetLevel = 0;
		this.FleetOneEquips = new List<int>();
		this.FleetOneExSlots = new List<int>();
		this.FleetOneTypes = new List<int>();
		this.FleetTwoEquips = new List<int>();
		this.FleetTwoExSlots = new List<int>();
		this.FleetTwoTypes = new List<int>();
	}

	/// <summary>
	/// Prepare fleet data
	/// </summary>
	/// <param name="fleetData"></param>
	/// <returns></returns>
	private List<TsunDbShipData> PrepareFleet(FleetData fleetData)
	{
		List<TsunDbShipData> shipsData = new List<TsunDbShipData>();

		IEnumerable<IShipData> members = fleetData.MembersWithoutEscaped?.Where(s => s != null).Cast<IShipData>().ToList() ?? new List<IShipData>();

		FleetSpeed = Math.Min(FleetSpeed, members.Select(s => s.Speed).Min());

		foreach (int weight in new int[4] { 1, 2, 3, 4 })
		{
			this.LOS[weight - 1] += Calculator.GetSearchingAbility_New33(fleetData, weight, this.HqLvl);
		}

		foreach (IShipData ship in fleetData.MembersInstance)
		{
			if (ship is null) continue;

			shipsData.Add(new TsunDbShipData(ship)
			{
				Flee = fleetData.EscapedShipList.Contains(ship.MasterID)
			});
		}

		return shipsData;
	}
	#endregion
}
