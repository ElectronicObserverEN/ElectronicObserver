using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.TsunDbSubmission.Battle;

/// <summary>
/// Contains information about the Air Base
/// </summary>
public class TsunDbBattleLBASData : TsunDbEntity
{
	/// <summary>
	/// Array of mst id of each plane in the base
	/// </summary>
	[JsonPropertyName("planes")]
	public List<int> PlaneIds { get; }

	/// <summary>
	/// Array of plane count of each plane in the base
	/// </summary>
	[JsonPropertyName("slots")]
	public List<int> PlaneCounts { get; }

	/// <summary>
	/// Array of plane proficiencies for each plane in the base
	/// </summary>
	[JsonPropertyName("proficiency")]
	public List<int> PlaneProfiencies { get; }

	/// <summary>
	/// Array of plane improvements for each plane in the base, -1 if none
	/// </summary>
	[JsonPropertyName("improvements")]
	public List<int> PlaneImprovements { get; }

	/// <summary>
	/// Array of plane morale for each plane in the base pre-sortie
	/// </summary>
	[JsonPropertyName("morale")]
	public List<int> PlaneMorale { get; }

	/// <summary>
	/// Array of int[2] of edges targeted by the land base
	/// </summary>
	[JsonPropertyName("strikepoints")]
	public List<int> StrikePoints { get; }

	protected override string Url => throw new NotImplementedException();

	public TsunDbBattleLBASData(IBaseAirCorpsData airCorp)
	{
		PlaneIds = airCorp.Squadrons.Values.Select(squadron => squadron.EquipmentID).ToList();
		PlaneCounts = airCorp.Squadrons.Values.Select(squadron => squadron.AircraftCurrent).ToList();

		PlaneProfiencies = airCorp.Squadrons.Values.Select(squadron => squadron.EquipmentInstance?.AircraftLevel ?? -1).ToList();
		PlaneImprovements = airCorp.Squadrons.Values.Select(squadron => squadron.EquipmentInstance?.Level ?? -1).ToList();

		PlaneMorale = airCorp.Squadrons.Values.Select(squadron => squadron.Condition).ToList();
		StrikePoints = airCorp.StrikePoints;
	}
}
