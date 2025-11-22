using System;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Core.Types.Mocks;

public class BaseAirCorpsSquadronMock : IBaseAirCorpsSquadron
{
	public int SquadronID { get; set; }
	public int State { get; set; }
	public int EquipmentMasterID { get; set; }
	public IEquipmentData? EquipmentInstance { get; init; }
	public int EquipmentID => (int?)EquipmentInstance?.EquipmentId ?? 0;
	public IEquipmentDataMaster? EquipmentInstanceMaster => EquipmentInstance?.MasterEquipment;
	public int AircraftCurrent { get; set; }
	public int AircraftMax { get; set; }
	public AirBaseCondition Condition { get; set; } = AirBaseCondition.Normal;
	public DateTime RelocatedTime { get; set; }
	public int ID { get; set; }
	public bool IsAvailable { get; set; }

	public BaseAirCorpsSquadronMock()
	{
		State = 0;
	}

	public BaseAirCorpsSquadronMock(IEquipmentData equipment) : this()
	{
		State = 1;
		EquipmentInstance = equipment;
		AircraftCurrent = equipment.MasterEquipment.AirBaseAircraftCount();
		AircraftMax = equipment.MasterEquipment.AirBaseAircraftCount();
	}

	public void LoadFromResponse(string apiname, object elem)
	{
		throw new NotImplementedException();
	}
}
