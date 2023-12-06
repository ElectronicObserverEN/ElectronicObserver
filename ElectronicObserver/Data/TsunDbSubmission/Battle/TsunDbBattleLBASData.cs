using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission.Battle;

/// <summary>
/// Contains information about the Air Base
/// </summary>
public class TsunDbBattleLBASData : TsunDbEntity
{
	/// <summary>
	/// Array of mst id of each plane in the base
	/// </summary>
	[JsonProperty("planes")]
	public List<int> PlaneIds { get; private set; }

	/// <summary>
	/// Array of plane count of each plane in the base
	/// </summary>
	[JsonProperty("slots")]
	public List<int> PlaneCounts { get; private set; }

	/// <summary>
	/// Array of plane proficiencies for each plane in the base
	/// </summary>
	[JsonProperty("proficiency")]
	public List<int> PlaneProfiencies { get; private set; }

	/// <summary>
	/// Array of plane improvements for each plane in the base, -1 if none
	/// </summary>
	[JsonProperty("improvements")]
	public List<int> PlaneImprovements { get; private set; }

	/// <summary>
	/// Array of plane morale for each plane in the base pre-sortie
	/// </summary>
	[JsonProperty("morale")]
	public List<int> PlaneMorale { get; private set; }

	/// <summary>
	/// Array of int[2] of edges targeted by the land base
	/// </summary>
	[JsonProperty("strikepoints")]
	public List<int> StrikePoints { get; private set; }

	protected override string Url => throw new NotImplementedException();

	public TsunDbBattleLBASData(BaseAirCorpsData airCorp)
	{
		PlaneIds = airCorp.Squadrons.Values.Select(squadron => squadron.EquipmentID).ToList();
		PlaneCounts = airCorp.Squadrons.Values.Select(squadron => squadron.AircraftCurrent).ToList();

		PlaneProfiencies = airCorp.Squadrons.Values.Select(squadron => squadron.EquipmentInstance?.AircraftLevel ?? -1).ToList();
		PlaneImprovements = airCorp.Squadrons.Values.Select(squadron => squadron.EquipmentInstance?.Level ?? -1).ToList();

		PlaneMorale = airCorp.Squadrons.Values.Select(squadron => squadron.Condition).ToList();
		StrikePoints = airCorp.StrikePoints;
	}
}
