using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ElectronicObserverTypes.Mocks;

public class FleetDataMock : IFleetData
{
	public int FleetID { get; set; }
	public string Name { get; set; }
	public FleetType FleetType { get; set; }
	public int ExpeditionState { get; set; }
	public int ExpeditionDestination { get; set; }
	public DateTime ExpeditionTime { get; set; }
	public ReadOnlyCollection<int> Members { get; set; }
	public ReadOnlyCollection<IShipData?>? MembersInstance { get; set; }
	public ReadOnlyCollection<IShipData?>? MembersWithoutEscaped => MembersInstance switch
	{
		{ } ships => Array.AsReadOnly(ships
			.Select(s => EscapedShipList.Contains(s?.MasterID ?? -1) switch
			{
				true => null,
				false => s,
			})
			.ToArray()),

		_ => null,
	};
	public ReadOnlyCollection<int> EscapedShipList { get; set; } = new(Array.Empty<int>());
	public bool IsInSortie { get; set; }
	public bool IsInPractice { get; set; }
	public int ID { get; set; }
	public int SupportType { get; set; }
	public bool IsFlagshipRepairShip { get; set; }
	public bool CanAnchorageRepair { get; set; }
	public DateTime? ConditionTime { get; set; }
	public Dictionary<string, string> RequestData { get; set; }
	public dynamic RawData { get; set; }
	public bool IsAvailable { get; set; }

	public int this[int i]
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new NotImplementedException();
	}

	public void LoadFromRequest(string apiname, Dictionary<string, string> data)
	{
		throw new NotImplementedException();
	}

	public void Escape(int index)
	{
		throw new NotImplementedException();
	}

	public int GetAirSuperiority()
	{
		throw new NotImplementedException();
	}

	public string GetAirSuperiorityString()
	{
		throw new NotImplementedException();
	}

	public double GetSearchingAbility()
	{
		throw new NotImplementedException();
	}

	public string GetSearchingAbilityString(int branchWeight = 1)
	{
		throw new NotImplementedException();
	}

	public double GetContactProbability()
	{
		throw new NotImplementedException();
	}

	public Dictionary<int, double> GetContactSelectionProbability()
	{
		throw new NotImplementedException();
	}

	public void UpdateConditionTime()
	{
		throw new NotImplementedException();
	}
}
