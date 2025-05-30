﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Data.TsunDbSubmission.Battle;

/// <summary>
/// Contains information about the sortied fleet(s), LBAS and support
/// </summary>
public class TsunDbFleetsAndAirBaseData : TsunDbEntity
{
	protected override string Url => throw new NotImplementedException();

	#region JSON properties
	/// <summary>
	/// Fleet 1 data (first sortied fleet)
	/// </summary>
	[JsonPropertyName("fleet1")]
	public List<TsunDbBattleShipData> Fleet1 { get; }

	/// <summary>
	/// Follows fleet1 or null if not in combined fleet
	/// </summary>
	[JsonPropertyName("fleet2")]
	public List<TsunDbBattleShipData>? Fleet2 { get; }

	/// <summary>
	/// Node/boss support fleet, null if no such fleet assigned
	/// </summary>
	[JsonPropertyName("support")]
	public List<TsunDbBattleShipData>? SupportFleet { get; }

	/// <summary>
	/// Array of JSON containing land base information
	/// </summary>
	[JsonPropertyName("lbas")]
	public List<TsunDbBattleLBASData> AirCorps { get; }

	/// <summary>
	/// Combined fleet type, corresponds to api_combined_flag
	/// </summary>
	[JsonPropertyName("fleettype")]
	public int FleetType { get; }
	#endregion

	#region Constructor
	public TsunDbFleetsAndAirBaseData()
	{
		KCDatabase db = KCDatabase.Instance;

		int sortiedFleet = db.Fleet.Fleets
			.Where(fleet => fleet.Value.IsInSortie)
			.Select(fleet => fleet.Value.FleetID)
			.Min();

		// --- Get the fleet type, if first fleet => flag of the combined fleet, else 0 (single fleet & strike force)
		FleetType = sortiedFleet == 1 ? (int)db.Fleet.CombinedFlag : 0;

		// --- Fleet 1
		Fleet1 = PrepareFleet(db.Fleet[sortiedFleet]);

		// --- Fleet 2
		if (FleetType > 0 && sortiedFleet == 1)
		{
			Fleet2 = PrepareFleet(db.Fleet[2]);
		}

		// --- Support fleet 
		if (db.Battle.Compass.IsBossNode)
		{
			if (db.Fleet.BossSupportFleetInstance != null)
			{
				SupportFleet = PrepareFleet(db.Fleet.BossSupportFleetInstance);
			}
		}
		else if (db.Fleet.NodeSupportFleetInstance != null)
		{
			SupportFleet = PrepareFleet(db.Fleet.NodeSupportFleetInstance);
		}

		AirCorps = db.BaseAirCorps.Values
			.Where(airCorp => airCorp.MapAreaID == db.Battle.Compass.MapAreaID)
			.Select(airCorp => new TsunDbBattleLBASData(airCorp))
			.ToList();
	}
	#endregion

	#region Private methods
	/// <summary>
	/// Prepare a fleet and returns it
	/// </summary>
	/// <param name="fleetData"></param>
	/// <returns></returns>
	private List<TsunDbBattleShipData> PrepareFleet(IFleetData fleetData) => fleetData.MembersInstance
		.Where(s => s != null)
		.Cast<IShipData>()
		.Select(ship => new TsunDbBattleShipData(ship, fleetData))
		.ToList();
	#endregion
}
