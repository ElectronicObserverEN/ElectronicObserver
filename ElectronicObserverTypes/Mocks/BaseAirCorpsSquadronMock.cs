using System;

namespace ElectronicObserverTypes.Mocks;

public class BaseAirCorpsSquadronMock : IBaseAirCorpsSquadron
{
	public int SquadronID { get; set; }
	public int State { get; set; } = 1;
	public int EquipmentMasterID { get; set; }
	public IEquipmentData? EquipmentInstance { get; set; }
	public int EquipmentID { get; set; }
	public IEquipmentDataMaster? EquipmentInstanceMaster { get; set; }
	public int AircraftCurrent { get; set; }
	public int AircraftMax { get; set; }
	public int Condition { get; set; }
	public DateTime RelocatedTime { get; set; }
	public int ID { get; set; }
	public bool IsAvailable { get; set; }
	public void LoadFromResponse(string apiname, object elem)
	{
		throw new NotImplementedException();
	}
}
